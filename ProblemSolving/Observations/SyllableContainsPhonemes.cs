using System.Collections.Generic;
using Starship.Core.ProblemSolving;
using Starship.Language.Phonetics;

namespace Starship.Language.ProblemSolving.Observations {
    public class SyllableContainsPhonemes : Observation {

        public SyllableContainsPhonemes(List<Phoneme> phonemes) {
            Phonemes = phonemes;
        }

        public List<Phoneme> Phonemes { get; set; }
    }
}