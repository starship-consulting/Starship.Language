using System.Collections.Generic;
using System.Linq;
using Starship.Core.ProblemSolving;
using Starship.Language.ProblemSolving.Conclusions;
using Starship.Language.ProblemSolving.Observations;

namespace Starship.Language.ProblemSolving.Analyzers {
    public class PhonemePatternAnalyzer : Analyzer<Word> {

        protected override void Analyze(List<Observation> observations, List<Constraint<Word>> constraints) {
            var matches = observations.OfType<SyllableContainsPhonemes>();

            foreach (var match in matches) {
                foreach (var phoneme in match.Phonemes) {
                    constraints.Add(new SyllableMustContainPhoneme(phoneme));
                }
            }

            // Determine which conclusions are violated whenever a new conclusion is added?
        }
    }
}