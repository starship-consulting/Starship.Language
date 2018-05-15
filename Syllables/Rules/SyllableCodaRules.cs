using System;
using Starship.Language.Phonetics;
using Starship.Language.Syllables.Rules;

namespace CrowdCode.Library.Modules.Language.Syllables.Rules {
    public class SyllableCodaRules : SyllableRule {
        public override RuleEvaluation ShouldAddPhoneme(Phoneme phoneme, SyllableContext context) {
            if (context.CurrentSyllable.HasNucleus()) {
                
                if (phoneme.IsVowel()) {
                    return False("Cannot have more than one vowel.");
                }

                var next = context.Phonemes.Next;
                var previous = context.Phonemes.Previous;

                if (previous == null || next == null) {
                    return True();
                }

                var isLast = context.Phonemes.Next == context.Phonemes.Last;
                var isFirst = context.Phonemes.Previous == context.Phonemes.First;
                
                if (phoneme.ArticulationManner == ArticulationManners.Aspirate) {
                    return False($"Coda cannot contain the aspirate '{phoneme.Letters}'");
                }

                if (previous.Text == "ay") {
                    return False("Syllables with 'ay' nucleus do not have coda.");
                }

                if (previous.Text == "er" || (phoneme.Text == "l" && next.HasVowel())) {
                    return False("'er' or 'l' is always end of coda.");
                }

                if (phoneme.ArticulationManner == ArticulationManners.Fricative && phoneme.ArticulationPlace == ArticulationPlaces.Alveolar) {
                    if (!context.CurrentSyllable.HasOnset() || phoneme.Text == "z") {
                        return False($"Syllable without onset cannot have coda that contains alveolar fricative '{phoneme.Letters}'");
                    }
                }

                if (phoneme.ArticulationManner == ArticulationManners.Liquid) {
                    return True();
                }

                if (isFirst && previous.IsVowel()) {
                    return True();
                }

                if (phoneme.ArticulationManner == ArticulationManners.Stop && next.ArticulationManner != ArticulationManners.Nasal) {
                    if (next.HasVowel() || (previous.ArticulationManner != ArticulationManners.Nasal && next.ArticulationManner != ArticulationManners.Stop)) {
                        return False("Onsets should begin with a stop unless adjacent to another stop or nasal.");
                    }
                }

                if (phoneme.ArticulationManner == ArticulationManners.Affricate && phoneme.ArticulationPlace == ArticulationPlaces.PostAlveolar) {
                    if (previous.ArticulationManner == ArticulationManners.Nasal) {
                        return False($"Affricate postalveolar '{phoneme.Letters}' cannot come after nasal '{previous.Letters}'.");
                    }
                }
                
                if (context.Syllables.Count == 0) {
                    if (context.CurrentSyllable.HasOnset() || (!isLast && !context.CurrentSyllable.HasPhonemeArticulationManner(ArticulationManners.Semivowel))) {
                        return True();
                    }
                }
                
                return False("Consonant is onset of next syllable.");
            }

            return True();
        }
    }
}