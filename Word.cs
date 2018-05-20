using System.Collections.Generic;
using System.Linq;
using Starship.Language.Interfaces;
using Starship.Language.Phonetics;
using Starship.Language.Syllables;

namespace Starship.Language {
    public class Word : HasSyllables {
        public Word() {
            Phonemes = new List<Phoneme>();
            Syllables = new List<Syllable>();
            NGrams = new List<OrderedWord>();
            Homographs = new List<Word>();
        }

        public Word(string text, List<Phoneme> phonemes) : this() {
            Text = text.ToLower();
            Phonemes = phonemes;

            InitializeSyllables();

            PhoneticSpeed = this.GetPhoneticSpeed();
        }

        public void InitializeSyllables() {
            Syllables = new SyllableMapper(Phonemes).GetSyllables();

            for (var index = 0; index < Syllables.Count; index++) {
                Syllables[index].AddWord(this, index);
            }
        }

        /*public Word(string text) : this() {
            if (text.IsEmpty()) {
                throw new Exception("Text cannot be null.");
            }

            text = text.ToLower();

            var word = English.GetWord(text);

            if (word == null) {
                throw new Exception("Unknown word: " + text);
            }

            Text = text;
            Phonemes = new List<Phoneme>(word.Phonemes);
            Syllables = word.Syllables;
            NGrams = word.NGrams;
        }*/

        /*public Word(string text, int syllables) : this(text) {
            if (Syllables.Count() < syllables) {
                for (var count = 0; count < syllables; count++) {
                    Syllables.Add(new Syllable());
                }
            }
        }*/

        /*public long GetNGramScore(int index) {
            var ngram = NGrams.FirstOrDefault(each => each.Index == index);
            return ngram?.NGram.Frequency ?? 0;
        }*/
        
        public IEnumerable<Word> GetHomographs() {
            yield return this;

            foreach (var word in Homographs) {
                yield return word;
            }
        }

        public long GetNGramScore() {
            return NGrams.Sum(each => each.NGram.Frequency);
        }

        public long GetNGramScore(int index) {
            return NGrams.Where(each => each.Index >= index).Sum(each => each.NGram.Frequency);
        }

        public string GetPhonemeText() {
            return string.Join(" ", Phonemes.Select(each => each.Text));
        }

        public string GetPhonemeLetters() {
            return string.Join(" ", Phonemes.Select(each => each.Letters));
        }

        public override string ToString() {
            var text = "[";

            for (var index = 0; index < Syllables.Count; index++) {
                var syllable = Syllables[index];

                foreach (var phoneme in syllable.Phonemes) {
                    text += phoneme + " ";
                }

                text = text.Trim();

                if (index < Syllables.Count - 1) {
                    text += "-";
                }
            }

            return $"{Text} ({text})";
        }

        public string Text { get; set; }

        public bool IsFast {
            get { return PhoneticSpeed <= 1; }
        }

        public int PhoneticSpeed { get; set; }

        public List<Word> Homographs { get; set; }

        public List<Syllable> Syllables { get; set; }

        public List<Phoneme> Phonemes { get; set; }

        public List<OrderedWord> NGrams { get; set; }
    }
}