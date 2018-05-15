using System;

namespace CrowdCode.Library.Modules.Language {
    public class ColoredText {

        public string Text { get; set; }
        
        public int Index { get; set; }

        public string Color { get { return ColorIndex[Index]; } }

        private static readonly string[] ColorIndex = {
            "#000000", "#2471A3", "#A93226", "#229954", "#D68910", "#45B39D"
        };
    }
}