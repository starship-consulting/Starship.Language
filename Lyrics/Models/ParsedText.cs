namespace Starship.Language.Lyrics.Models {
    public class ParsedText {

        public string Text { get; set; }

        public string CleanedText { get; set; }

        public RhymeGroup RhymeGroup { get; set; }

        public WordData Word { get; set; }

        public int Index { get; set; }

        public int Line { get; set; }
    }
}