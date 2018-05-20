using System.Collections.Generic;
using Starship.Language.Phonetics;
using Starship.Language.Syllables;

namespace Starship.Language.ProblemSolving {
    public class SyllableResult {

        public SyllableResult() {
            Syllables = new List<Syllable>();
            Phonemes = new List<Phoneme>();
        }
        
        public List<Syllable> Syllables { get; set; }

        public List<Phoneme> Phonemes { get; set; }
    }
}