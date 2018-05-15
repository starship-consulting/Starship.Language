using Starship.Language.Lyrics.Models;

namespace Starship.Language.Lyrics {
    public class WordEvaluator {
        public WordEvaluator(string word) {
            Word = word;
        }

        public RawWordData GetRawWordData() {
            return new RawWordData {
                Word = Word,
                Unknown = true,
                Syllables = new RawWordData.SyllableDefinition { Count = CountSyllables(Word) }
            };
        }

        private static int CountSyllables(string word) {
            word = word.ToLower().Trim();
            int count = System.Text.RegularExpressions.Regex.Matches(word, "[aeiouy]+").Count;
            if ((word.EndsWith("e") || (word.EndsWith("es") || word.EndsWith("ed"))) && !word.EndsWith("le"))
                count--;
            return count;
        }

        public string Word { get; set; }
    }
}