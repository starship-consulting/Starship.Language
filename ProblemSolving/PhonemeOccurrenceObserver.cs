using System.Collections.Generic;
using Starship.Core.ProblemSolving;
using Starship.Language.ProblemSolving.Observations;

namespace Starship.Language.ProblemSolving {
    public class PhonemeOccurrenceObserver : PatternObserver<Word> {
        public override void GetObservations(Word fact, List<Observation> observations) {
            foreach (var syllable in fact.Syllables) {
                observations.Add(new SyllableContainsPhonemes(syllable.Phonemes));
            }
        }
    }
}