using System.Collections.Generic;

namespace Starship.Language.Lyrics.Models {
    public class RawWordData {
        public RawWordData() {
            Results = new List<WordDefinition>();
            //Pronunciation = new Dictionary<string, object>();
        }

        public static RawWordData ForUnknown(string word) {
            return new WordEvaluator(word).GetRawWordData();
        }

        public List<string> GetSyllables() {
            if (Syllables == null) {
                return new List<string> { Word };
            }

            return Syllables.List;
        }

        public int CountSyllables() {
            if (Syllables != null) {
                return Syllables.Count;
            }

            return 1;
        }

        public string Word { get; set; }

        public bool Unknown { get; set; }

        public List<WordDefinition> Results { get; set; }

        public SyllableDefinition Syllables { get; set; }

        //public Dictionary<string, object> Pronunciation { get; set; }

        public decimal Frequency { get; set; }

        public class SyllableDefinition {
            public int Count { get; set; }

            public List<string> List { get; set; }
        }
    }
}