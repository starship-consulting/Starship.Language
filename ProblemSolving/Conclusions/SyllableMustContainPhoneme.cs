using System.Linq;
using CrowdCode.Library.Modules.Language;
using Starship.Core.ProblemSolving;
using Starship.Language.Phonetics;

namespace Starship.Language.ProblemSolving.Conclusions {
    public class SyllableMustContainPhoneme : Constraint<Word> {

        public SyllableMustContainPhoneme(Phoneme phoneme) {
            Phoneme = phoneme;
        }

        public override bool SatisfiesConstraint(Word input) {
            return input.Phonemes.Any(each => each.Id == Phoneme.Id);
        }

        public Phoneme Phoneme { get; set; }
    }
}