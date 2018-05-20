using System;
using System.Collections.Generic;
using System.Linq;
using Starship.Core.Extensions;
using Starship.Language.Enumerations;
using Starship.Language.Phonetics;

namespace Starship.Language.Syllables {
    public class Syllable {

        public Syllable() {
            Stress = -1;
            Phonemes = new List<Phoneme>();
            Words = new Dictionary<int, List<Word>>();
            Rhymes = new List<Syllable>();
        }

        public static Syllable Parse(string phonemes) {
            return new Syllable {
                Phonemes = Phoneme.Parse(phonemes)
            };
        }

        public IEnumerable<Phoneme> FromNucleus() {
            return Phonemes.SkipWhile(each => !each.IsVowel());
        }

        public bool RhymesWith(Word word) {
            return RhymeFinder.IsRhyme(this, word.Syllables.Last());
        }

        public bool RhymesWith(Syllable syllable) {
            return RhymeFinder.IsRhyme(this, syllable);
        }

        public bool HasPhoneme(Func<Phoneme, bool> predicate) {
            return Phonemes.Any(predicate);
        }

        public bool HasPhonemeArticulationManner(ArticulationManners manner) {
            return Phonemes.Any(phoneme => phoneme.ArticulationManner == manner);
        }

        public bool HasPhonemeConsonant() {
            return Phonemes.Any(phoneme => phoneme.ArticulationManner != ArticulationManners.Vowel);
        }

        public int CountPhonemeVowels() {
            return Phonemes.Count(phoneme => phoneme.ArticulationManner == ArticulationManners.Vowel);
        }

        public bool HasPhonemeVowel() {
            return HasPhonemeArticulationManner(ArticulationManners.Vowel);
        }

        public bool HasVowel() {
            return Phonemes.Any(phoneme => phoneme.Letters.HasVowel());
        }

        public bool HasOnset() {
            foreach (var phoneme in Phonemes) {
                if (phoneme.IsVowel() || phoneme.IsSemiVowel()) {
                    return false;
                }

                return true;
            }

            return false;
        }

        public bool HasNucleus() {
            return HasPhonemeVowel();
        }

        public SyllableNucleusTypes GetNucleus() {
            var partial = false;

            foreach (var phoneme in Phonemes) {
                if (phoneme.ArticulationManner == ArticulationManners.Vowel) {
                    if (phoneme.Text == "uh" || phoneme.Text == "ae" || phoneme.Text == "ow" || phoneme.Text == "ih" || phoneme.Text == "eh" /*|| phoneme.Text == "ah"*/) { // Nucleus vowels
                        partial = true;
                        continue;
                    }

                    return SyllableNucleusTypes.Full;
                }

                if (partial) {
                    return SyllableNucleusTypes.Full;
                }
            }

            return partial ? SyllableNucleusTypes.Partial : SyllableNucleusTypes.None;
        }

        public bool HasCoda(Phoneme currentPhoneme) {

            if (!Phonemes.Any()) {
                return false;
            }

            var last = Phonemes.Last();

            return HasNucleus() && !last.IsVowel() && last.Text != "s";

            /*if (currentPhoneme.ArticulationPlace == ArticulationPlaces.Alveolar && currentPhoneme.ArticulationManner == ArticulationManners.Fricative) {
                return false;
            }

            var nucleus = GetNucleus();
            
            if (nucleus == SyllableNucleusTypes.Full) {
                return true;
            }
            
            if (nucleus == SyllableNucleusTypes.Partial) {
                if (currentPhoneme.ArticulationManner == ArticulationManners.Liquid) {
                    //return true;
                }
            }
            
            return false;*/
        }

        public IEnumerable<Word> GetWords(int index = -1) {
            if (index < 0) {
                return Words.SelectMany(each => each.Value);
            }

            if (Words.ContainsKey(index)) {
                return Words[index];
            }

            return new List<Word>();
        }

        public void AddWord(Word word, int syllableIndex) {
            lock (Words) {
                if (!Words.ContainsKey(syllableIndex)) {
                    Words.Add(syllableIndex, new List<Word>());
                }

                Words[syllableIndex].Add(word);
            }
        }

        public string PhonemeString {
            get {
                return string.Join(" ", Phonemes.Select(each => each.ToString())).Trim();
            }
        }

        public string Text {
            get {
                return string.Join("", Phonemes.Select(each => each.Letters)).Trim();
            }
        }

        public string Id {
            get { return Text + "*" + PhonemeString; }
        }

        public override string ToString() {
            return Text + " (" + PhonemeString + ")";
        }

        public bool IsPrefix { get; set; }

        public int Stress { get; set; }
        
        public string Rule { get; set; }

        public List<Syllable> Rhymes { get; set; }

        public List<Phoneme> Phonemes { get; set; }

        private Dictionary<int, List<Word>> Words { get; set; }
    }
}