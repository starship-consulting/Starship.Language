using System.Collections.Generic;
using System.Linq;

namespace Starship.Language.Lyrics.Models {
    public class WordData {

        public WordData() {
            Definitions = new List<WordDefinition>();
            //Syllables = new List<string>();
            //Pronunciation = new Dictionary<string, object>();
            Rhymes = new List<string>();
        }

        public WordData(RawWordData rawData) : this() {
            Word = rawData.Word;
            IsUnknown = rawData.Unknown;
            Definitions = rawData.Results.ToList();
            Syllables = rawData.CountSyllables();
            //Pronunciation = rawData.Pronunciation.ToList();
            Frequency = rawData.Frequency;
        }

        public string Word { get; set; }

        public bool IsUnknown { get; set; }

        public List<WordDefinition> Definitions { get; set; }
        
        public int Syllables { get; set; }
        //public List<string> Syllables { get; set; }
        
        //public Dictionary<string, object> Pronunciation { get; set; }

        public List<string> Rhymes { get; set; }

        public decimal Frequency { get; set; }
    }
}