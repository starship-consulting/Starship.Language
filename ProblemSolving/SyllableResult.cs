using System;
using System.Collections.Generic;
using CrowdCode.Library.Modules.Language.Syllables;
using Starship.Language.Phonetics;
using Starship.Language.Syllables;

namespace CrowdCode.Library.Modules.Language.ProblemSolving {
    public class SyllableResult {

        public SyllableResult() {
            Syllables = new List<Syllable>();
            Phonemes = new List<Phoneme>();
        }
        
        public List<Syllable> Syllables { get; set; }

        public List<Phoneme> Phonemes { get; set; }
    }
}