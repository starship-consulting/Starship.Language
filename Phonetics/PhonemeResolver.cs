using System.Collections.Generic;
using System.Linq;
using Starship.Core.Extensions;

namespace Starship.Language.Phonetics {
    public class PhonemeResolver {
        public PhonemeResolver(string text) {
            Text = text.ToLower();
        }

        public void BindPhonemes(params Phoneme[] phonemes) {
            Phonemes = phonemes.ToList();

            /*if (Phonemes.Count == Text.Length) {
                for (var index = 0; index < Text.Length; index++) {
                    Phonemes[index].Letters += Text[index];
                }

                return;
            }*/

            for (CharacterIndex = 0; CharacterIndex < Text.Length; CharacterIndex++) {
                CurrentCharacter = Text[CharacterIndex].ToString();

                if (IsLastPhoneme || (IsPerfectMatch(CurrentPhoneme) && !IsPerfectMatch(NextPhoneme))) {
                    CurrentPhoneme.Letters += CurrentCharacter;
                    continue;
                }

                if (IsLastCharacter) {
                    FinishPhoneme("last character");
                }
                else if (NextPhonemeMatchesCurrentCharacter()) {
                    FinishPhoneme($"next phoneme '{NextPhoneme.Text}' matches '{CurrentCharacter}'");
                }

                if (SameAsPreviousCharacter()) {
                    if (CurrentPhoneme.Stress == 2 && CurrentPhoneme.Letters.Length > 0) {
                        FinishPhoneme("same as previous character");
                    }

                    CurrentPhoneme.Letters += CurrentCharacter;
                    continue;
                }

                //var unmatches = 0;

                while (true) {
                    if (IsLastPhoneme || CurrentCharacterMatchesPhoneme(CurrentPhoneme)) {
                        CurrentPhoneme.Letters += CurrentCharacter;
                        break;
                    }

                    FinishPhoneme("character '" + CurrentCharacter + "' does not match");
                    /*unmatches += 1;

                    if (unmatches == 3) {
                        throw new Exception($"Unable to place character '{CurrentCharacter}'");
                    }*/
                }

                if (CurrentCharacter == "x" && CurrentPhoneme.Text == "k") {
                    FinishPhoneme("character is 'x' and phoneme is 'k'");
                    continue;
                }

                //if (CurrentPhoneme.Letters.Length == 0) {
                //    throw new Exception("No letters set for phoneme: " + CurrentPhoneme);
                //}
            }
        }

        private bool IsPerfectMatch(Phoneme phoneme) {
            if (IsLastPhoneme) {
                return true;
            }

            return phoneme.IsDefiniteLetter(CurrentCharacter);
        }
        
        private bool SameAsPreviousCharacter() {
            return CharacterIndex > 0 && Text[CharacterIndex - 1].ToString() == CurrentCharacter;
        }

        private bool NextPhonemeMatchesCurrentCharacter() {
            return CurrentPhoneme.Letters.Length > 0 && CurrentCharacterMatchesPhoneme(NextPhoneme, false);
        }

        private bool IsLastCharacter { get { return CharacterIndex == Text.Length - 1; } }

        private bool CurrentCharacterMatchesPhoneme(Phoneme phoneme, bool allowVowels = true) {
            if (allowVowels && phoneme.Letters.Length == 0 && phoneme.HasVowel() && CurrentCharacter.HasVowel()) {
                return true;
            }

            return phoneme.GetPossibleLetters().Any(each => each == CurrentCharacter);
        }

        private void FinishPhoneme(string reason) {
            if (IsLastPhoneme) {
                return;
            }

            /*if (CurrentPhoneme.Letters.Length == 0) {
                throw new Exception("No letters set for phoneme: " + CurrentPhoneme);
            }*/

            CurrentPhoneme.Reason = reason;
            PhonemeIndex += 1;
        }

        public string Text { get; set; }

        private bool IsLastPhoneme {
            get { return Phonemes.Count - 1 == PhonemeIndex; }
        }

        private Phoneme NextPhoneme { get { return Phonemes[PhonemeIndex + 1]; } }

        private Phoneme CurrentPhoneme { get { return Phonemes[PhonemeIndex]; } }

        private Phoneme PreviousPhoneme { get { return Phonemes[PhonemeIndex - 1]; } }

        private string CurrentCharacter { get; set; }

        private int CharacterIndex { get; set; }

        private int PhonemeIndex { get; set; }

        private List<Phoneme> Phonemes { get; set; }
    }
}