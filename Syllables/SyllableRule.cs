using System;
using CrowdCode.Library.Modules.Language.Syllables.Rules;
using Starship.Language.Phonetics;
using Starship.Language.Syllables.Rules;

namespace CrowdCode.Library.Modules.Language.Syllables {
    public abstract class SyllableRule {
        public virtual RuleEvaluation ShouldAddPhoneme(Phoneme phoneme, SyllableContext context) {
            return True();
        }

        protected RuleEvaluation True() {
            return new RuleEvaluation {
                Outcome = true
            };
        }

        protected RuleEvaluation False(string reason) {
            return new RuleEvaluation {
                Outcome = false,
                Reason = reason
            };
        }
    }
}