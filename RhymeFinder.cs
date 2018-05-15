using System.Collections.Generic;
using System.Linq;
using CrowdCode.Library.Modules.Language;
using CrowdCode.Library.Modules.Language.Syllables;
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

            SyllableMapper.UniqueSyllables.Values.ForEachParallel(syllable => {
                for (var index = 0; index < word.Syllables.Count; index++) {
                    var match = word.Syllables[index];
                    
                    if (IsRhyme(match, syllable)) {
                        foreach (var rhymeWord in syllable.GetWords()) {
                            if (rhymeWord.Syllables.Count <= index + 1) {
                                lock (results) {
                                    if (!results.ContainsKey(index)) {
                                        results.Add(index, new List<Word>());
                                    }

                                    results[index].Add(rhymeWord);
                                }
                            }
                        }
                    }
                }
            });

            return results.Values.SelectMany(each => each).Select(each => each.Text).ToList();
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

        public static bool IsRhyme(Word word1, Word word2) {
            var permutations = word1.GetHomographs().ForEachPermutation(word2.GetHomographs());

            foreach (var set in permutations) {
                if (IsRhyme(set.Item1.Syllables.Last(), set.Item2.Syllables.Last())) {
                    return true;
                }
            }

            return false;
        }

        public static bool IsRhyme(Syllable syllable1, Syllable syllable2) {
            var phonemes1 = syllable1.FromNucleus().ToList();
            var phonemes2 = syllable2.FromNucleus().ToList();

            if (phonemes1.Count == 0 || phonemes2.Count == 0) {
                return false;
            }

            //l aa k s
            //b aa k s

            // Remove 'ng' from 'ing' words
            /*if (phonemes1.Last().Text.ToLower() == "ng") {
                phonemes1 = phonemes1.Take(phonemes1.Count - 1).ToList();
            }
            if (phonemes2.Last().Text.ToLower() == "ng") {
                phonemes2 = phonemes2.Take(phonemes2.Count - 1).ToList();
            }*/

            List<Phoneme> iterator1;
            List<Phoneme> iterator2;

            if (phonemes1.Count > phonemes2.Count) {
                iterator1 = phonemes1;
                iterator2 = phonemes2;
            }
            else {
                iterator1 = phonemes2;
                iterator2 = phonemes1;
            }

            var limit = iterator2.Count - 1;

            for (var index = 0; index < iterator1.Count; index++) {
                var phoneme1 = iterator1[index];
                var phoneme2 = iterator2[index > limit ? limit : index];

                if (iterator2.Count < index + 1) {
                    if (index > 0) {
                        var compareTo = iterator1[index - 1];

                        //if (phoneme1.IsSilent(compareTo)) {
                        if (phoneme1.IsSilent()) {
                            continue;
                        }
                    }

                    return false;
                }

                if (!phoneme1.SoundsLike(phoneme2)) {
                    return false;
                }
            }

            return true;
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