using System.Collections.Generic;
using System.Linq;
using CrowdCode.Library.Modules.Language.Syllables;
using Starship.Core.Collections;
using Starship.Language.Phonetics;

namespace Starship.Language.Syllables.Rules {
    public class SyllableContext {

        public SyllableContext(List<Phoneme> phonemes) {
            CurrentSyllable = new Syllable();
            Syllables = new List<Syllable>();
            Phonemes = new SteppableList<Phoneme>(phonemes);
        }

        /*public int RemainingSyllables() {
            return Phonemes.GetRemaining().Count(each => each.IsVowel());
        }*/

        public int RemainingVowels() {
            return Phonemes.GetRemaining().Count(each => each.IsVowel());
        }

        public int TotalVowels() {
            return Syllables.Sum(each => each.CountPhonemeVowels());
        }

        public int TotalSyllables { get { return Syllables.Count; } }

        public Syllable CurrentSyllable { get; set; }

        public List<Syllable> Syllables { get; set; }

        public SteppableList<Phoneme> Phonemes { get; set; }
    }
}