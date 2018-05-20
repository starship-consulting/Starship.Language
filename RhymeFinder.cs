using System.Collections.Generic;
using System.Linq;
using Starship.Core.Extensions;
using Starship.Language.Phonetics;
using Starship.Language.Syllables;

namespace Starship.Language {
    public static class RhymeFinder {

        public static List<Word> GetRhymes(Word word) {
            return GetRhymes(word.Syllables.Last());
        }

        public static List<string> GetMultiRhymes(Word word) {
            var results = new Dictionary<int, List<Word>>();
            var rhymedSyllables = new Dictionary<int, List<Syllable>>();

            for (var index = 0; index < word.Syllables.Count; index++) {
                results.Add(index+1, new List<Word>());
                rhymedSyllables.Add(index, new List<Syllable>());
            }
            
            SyllableMapper.UniqueSyllables.Values.ForEachParallel(eachUniqueSyllable => {
                for (var eachSyllableIndex = 0; eachSyllableIndex < word.Syllables.Count; eachSyllableIndex++) {

                    var syllable = word.Syllables[eachSyllableIndex];
                    
                    if (IsRhyme(syllable, eachUniqueSyllable)) {
                        
                        rhymedSyllables[eachSyllableIndex].Add(eachUniqueSyllable);
                    }
                }
            });

            foreach(var syllableSet in rhymedSyllables) {
                var index = syllableSet.Key;

                foreach(var syllable in syllableSet.Value) {
                    foreach(var eachWord in syllable.GetWords(0)) {
                        if (eachWord.Syllables.Count <= index + 1) {

                            if(eachWord.GetNGramScore(index) <= 0) {
                                continue;
                            }

                            var count = 0;
                            var match = true;

                            foreach(var eachSyllable in eachWord.Syllables.Skip(1)) {
                                count += 1;

                                if(!rhymedSyllables[count].Contains(eachSyllable)) {
                                    match = false;
                                    break;
                                }
                            }

                            if(match){
                                results[index+1].Add(eachWord);
                            }
                        }
                    }
                }
            }
            
            var strict = true;
            
            var matches = FindMultiSyllables(results, word.Syllables.Count).ToList();

            return matches.Where(each => each.GetNGramScore() > 0).Select(each => each.Text).ToList();
        }

        private static IEnumerable<Phrase> FindMultiSyllables(Dictionary<int, List<Word>> wordlist, int syllables, int level = 0) {
            foreach (var set in wordlist) {
                var required = set.Key - level;

                if (required <= 0) {
                    continue;
                }

                foreach (var eachWord in set.Value.Where(each => each.Syllables.Count == required).ToList()) {
                    var lookAhead = required + level;

                    if (syllables == lookAhead) {
                        yield return new Phrase(eachWord);
                        continue;
                    }

                    var words = FindMultiSyllables(wordlist, syllables, lookAhead).ToList();

                    foreach (var word in words) {
                        var combined = new List<Word>();
                        combined.Add(eachWord);
                        combined.AddRange(word.Words);

                        yield return new Phrase(combined.ToArray());
                    }
                }
            }
        }

        public static List<Word> GetRhymes(Syllable match, int maxSyllables = 0) {
            var results = new List<Word>();

            return GetPhonemeRhymes(match, maxSyllables);

            /*SyllableMapper.Syllables.Values.ForEachParallel(syllable => {
                if (IsRhyme(match, syllable)) {
                    foreach (var rhymeWord in syllable.Words) {
                        if ((maxSyllables == 0 || rhymeWord.Syllables.Count <= maxSyllables) && rhymeWord.Syllables.Last() == syllable) {
                            lock (results) {
                                results.Add(rhymeWord);
                            }
                        }
                    }
                }
            });*/

            return results;
        }

        public static void BuildRhymes() {
            /*var total = SyllableMapper.Syllables.Count;
            var finished = 0;

            SyllableMapper.Syllables.Values.ForEachParallel(syllable1 => {
                foreach (var syllable2 in SyllableMapper.Syllables.Values) {
                    if (IsRhyme(syllable1, syllable2)) {
                        syllable1.Rhymes.Add(syllable2);
                        syllable2.Rhymes.Add(syllable1);
                    }
                }

                finished += 1;
            });*/

            /*foreach (var set in SyllableMapper.Syllables.Values.ForEachPermutation()) {
                if (IsRhyme(set.Item1.Phonemes, set.Item2.Phonemes)) {
                    set.Item1.Rhymes.Add(set.Item2);
                    set.Item2.Rhymes.Add(set.Item1);
                }
            }*/
        }

        public static bool IsRhyme(string word1, string word2) {
            return IsRhyme(English.GetWord(word1), English.GetWord(word2));
        }

        public static bool IsRhyme(Word word1, Word word2, out string reason) {
            var permutations = word1.GetHomographs().ForEachPermutation(word2.GetHomographs());
            
            reason = string.Empty;

            foreach (var set in permutations) {
                if (IsRhyme(set.Item1.Syllables.Last(), set.Item2.Syllables.Last(), out reason)) {
                    return true;
                }
            }
            
            return false;
        }

        public static bool IsRhyme(Word word1, Word word2) {
            return IsRhyme(word1, word2, out _);
        }

        public static bool IsRhyme(Syllable syllable1, Syllable syllable2, out string reason) {
            var phonemes1 = syllable1.FromNucleus().ToList();
            var phonemes2 = syllable2.FromNucleus().ToList();

            if (phonemes1.Count == 0 || phonemes2.Count == 0) {
                reason = "No phonemes found.";
                return false;
            }

            List<Phoneme> mostPhonemes;
            List<Phoneme> lessPhonemes;

            if (phonemes1.Count > phonemes2.Count) {
                mostPhonemes = phonemes1;
                lessPhonemes = phonemes2;
            }
            else {
                mostPhonemes = phonemes2;
                lessPhonemes = phonemes1;
            }

            var limit = lessPhonemes.Count - 1;

            for (var index = 0; index < mostPhonemes.Count; index++) {
                var phoneme1 = mostPhonemes[index];
                var phoneme2 = lessPhonemes[index > limit ? limit : index];

                if (lessPhonemes.Count < index + 1) {
                    if(mostPhonemes[index-1].Id != phoneme2.Id) {
                        reason = "Invalid number of phonemes";
                        return false;
                    }
                }

                if (!phoneme1.SoundsLike(phoneme2)) {
                    reason = "Phonemes do not sound alike.";
                    return false;
                }
            }

            reason = string.Empty;
            return true;
        }

        public static bool IsRhyme(Syllable syllable1, Syllable syllable2) {
            return IsRhyme(syllable1, syllable2, out _);
        }

        public static List<Word> GetPhonemeRhymes(Syllable syllable, int maxSyllables = 0) {
            var rhymes = new Dictionary<string, Word>();

            foreach (var word in English.GetWords()) {

                // todo:  Temporary hack, this should be removed
                //if (word.Syllables.Count <= 0) {
                //    continue;
                //}

                if (rhymes.ContainsKey(word.Text)) {
                    continue;
                }

                if (maxSyllables > 0 && word.Syllables.Count > maxSyllables) {
                    continue;
                }

                if (word.Text.Trim().Length <= 1 || word.Text.Contains(".")) {
                    continue;
                }

                if (IsRhyme(syllable, word.Syllables.Last())) {
                    rhymes.Add(word.Text, word);
                }
            }

            //return rhymes.GroupBy(each => each.Value).SelectMany(each => each.ToList()).OrderByDescending(each => each.Value).Select(each => each.Key).ToList();
            return rhymes.Values.ToList();
        }
    }
}