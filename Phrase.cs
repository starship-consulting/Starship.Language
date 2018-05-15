using System;
using System.Collections.Generic;
using System.Linq;
using CrowdCode.Library.Modules.Language.Interfaces;
using CrowdCode.Library.Modules.Language.NGrams;
using CrowdCode.Library.Modules.Language.Syllables;
using Starship.Language;
using Starship.Language.Phonetics;
using Starship.Language.Syllables;

namespace CrowdCode.Library.Modules.Language {
    public class Phrase : HasSyllables {
        public Phrase() {
            Words = new List<Word>();
        }

        public Phrase(params Word[] words) {
            Words = words.ToList();
            Initialize();
        }

        public Phrase(string text) : this() {
            foreach (var word in text.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries)) {
                var english = English.GetWord(word);

                if (english == null) {
                    Words.Clear();
                    break;
                }

                Words.Add(english);
            }

            Initialize();
        }

        private void Initialize() {
            foreach (var word in Words) {
                if (word.PhoneticSpeed > PhoneticSpeed) {
                    PhoneticSpeed = word.PhoneticSpeed;
                }
            }

            if (PhoneticSpeed <= 1) {
                PhoneticSpeed = this.GetPhoneticSpeed();
            }
        }

        public IEnumerable<Phrase> GetRhymes(bool strict = true) {
            if (Syllables.Count > 10) {
                throw new Exception("Too many syllables.");
            }

            var rhymes = InternalGetRhymes(strict).Where(rhyme => {
                var index = 0;

                foreach (var word in Words) {
                    if (word.Text == rhyme.Words[index].Text && RhymeFinder.IsRhyme(word, rhyme.Words[index])) {
                        return false;
                    }

                    if (rhyme.Words.Count > index + 1) {
                        index += 1;
                    }
                }

                return true;
            })
            .Select(rhyme => {
                var consecutive = 0;
                var count = 0;
                var index = rhyme.Words.SelectMany(each => each.Syllables).Count() - 1;
                var isConsecutive = true;

                foreach (var syllable in rhyme.Words.SelectMany(each => each.Syllables).Reverse()) {
                    if (RhymeFinder.IsRhyme(syllable, Syllables[index])) {
                        if (isConsecutive) {
                            consecutive += 1;
                        }

                        count += 1;
                    }
                    else {
                        isConsecutive = false;
                    }

                    index -= 1;
                }

                return new {
                    Rhyme = rhyme,
                    Consecutive = consecutive,
                    Count = count
                };
            })
            //.Where(each => each.Consecutive == each.Rhyme.Words.Sum(w => w.Syllables.Count))
            .OrderByDescending(each => each.Count)
            .ThenByDescending(each => each.Consecutive)
            .ThenByDescending(each => each.Rhyme.GetNGramScore())
            .Take(500)
            .Select(each => each.Rhyme)
            .ToList();

            return rhymes;
        }

        /*public Rhyme EvaluateRhyme(Phrase word) {
            var consecutive = 0;
            var count = 0;
            var index = word.Words.SelectMany(each => each.Syllables).Count() - 1;
            var isConsecutive = true;

            foreach (var syllable in word.Words.SelectMany(each => each.Syllables).Reverse()) {
                if (RhymeFinder.IsRhyme(syllable.Phonemes, Syllables[index].Phonemes)) {
                    if (isConsecutive) {
                        consecutive += 1;
                    }

                    count += 1;
                }
                else {
                    isConsecutive = false;
                }

                index -= 1;
            }

            return new Rhyme {
                Phrase = word,
                Consecutive = consecutive,
                Count = count
            };
        }*/

        private IEnumerable<Phrase> InternalGetRhymes(bool strict = true) {
            var index = 1;
            var results = new Dictionary<int, List<Word>>();
            var ngrams = new Dictionary<string, NGram>();

            foreach (var syllable in Syllables) {
                var rhymes = RhymeFinder.GetRhymes(syllable, index).ToList();

                var validWords = new List<Word>();

                foreach (var rhyme in rhymes.Where(each => index - each.Syllables.Count + 1 == 1).ToList()) {
                    validWords.Add(rhyme);

                    if (strict) {
                        foreach (var ngram in rhyme.NGrams.Where(each => each.Index == 1)) {
                            if (!ngrams.ContainsKey(ngram.NGram.Text)) {
                                ngrams.Add(ngram.NGram.Text, ngram.NGram);
                            }
                        }
                    }
                }

                foreach (var rhyme in rhymes.Where(each => index - each.Syllables.Count + 1 != 1).ToList()) {
                    if (strict) {
                        var requiredSyllables = index - rhyme.Syllables.Count;

                        foreach (var ngram in rhyme.NGrams.OrderByDescending(each => each.NGram.Frequency)) {
                            if (ngrams.ContainsKey(ngram.NGram.Text)) {
                                var priorSyllables = ngram.NGram.Words.Where(each => each.Index < ngram.Index).Sum(each => each.Word.Syllables.Count);

                                if (priorSyllables == requiredSyllables) {
                                    validWords.Add(rhyme);

                                    // Todo:  Should filter all valid NGrams for all permutations
                                    break;
                                }
                            }
                        }
                    }
                    else {
                        validWords.Add(rhyme);
                    }
                }

                var bestResults = validWords.OrderByDescending(each => each.NGrams.Sum(ngram => ngram.NGram.Frequency)) /*.Take(100)*/.ToList();

                results.Add(index, bestResults);

                index += 1;
            }

            var final = FindMultiSyllables(results, Syllables.Count);

            if (strict) {
                final = final.Where(each => each.GetNGramScore() > 0);
            }

            return final;
        }

        private IEnumerable<Phrase> FindMultiSyllables(Dictionary<int, List<Word>> wordlist, int syllables, int level = 0) {
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

        public long GetNGramScore() {
            if (Text.Length == 0) {
                return 0;
            }

            var collection = English.GetNGrams(Words.Count);

            if (collection == null || !collection.ContainsKey(Text)) {
                return 0;
            }

            return collection[Text].Frequency;
        }

        public string Text {
            get { return string.Join(" ", Words.Select(each => each.Text)).Trim(); }
        }

        public int PhoneticSpeed { get; set; }

        public bool IsFast {
            get { return PhoneticSpeed <= 1; }
        }

        public List<Word> Words { get; set; }

        public List<Syllable> Syllables {
            get { return Words.SelectMany(each => each.Syllables).ToList(); }
        }

        public List<Phoneme> Phonemes {
            get { return Words.SelectMany(each => each.Phonemes).ToList(); }
        }

        public override string ToString() {
            return Text;
        }
    }
}