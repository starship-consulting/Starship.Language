using Starship.Language.Phonetics;

namespace Starship.Language.Syllables.Rules {
    public class SyllableMustMaintainSonority : SyllableRule {

        public override RuleEvaluation ShouldAddPhoneme(Phoneme phoneme, SyllableContext context) {
            if (context.Phonemes.Previous == null) {
                return True();
            }
            
            // Sonority must always be decreasing once we have a nucleus
            if (context.CurrentSyllable.HasNucleus()) {
                var previous = context.Phonemes.Previous;

                if (phoneme.Definition.Sonority > previous.Definition.Sonority) {
                    return False($"Phoneme '{phoneme.Letters}' has more sonority ({phoneme.Definition.Sonority}) than previous '{previous.Letters}' sonority ({previous.Definition.Sonority}).");
                }
            }
            
            return True();
        }
    }
}