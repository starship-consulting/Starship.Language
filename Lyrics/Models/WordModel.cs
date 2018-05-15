using Newtonsoft.Json;

namespace Starship.Language.Lyrics.Models {
    public class WordModel {

        public WordModel(ParsedText text) {
            Word = text.Text;
            IsUnknown = text.Word.IsUnknown;

            if (text.RhymeGroup != null) {
                Color = text.RhymeGroup.Color;
                Group = text.RhymeGroup.Index;
            }

            Index = text.Index;
            Syllables = text.Word.Syllables;
            Line = text.Line;
        }

        public string Word { get; set; }

        public int Syllables { get; set; }
        
        public string Color { get; set; }

        public bool IsUnknown { get; set; }

        public int Group { get; set; }

        public int Index { get; set; }

        public int Line { get; set; }

        public override string ToString() {
            return JsonConvert.SerializeObject(this);
        }
    }
}