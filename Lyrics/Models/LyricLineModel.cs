using System.Collections.Generic;
using System.Linq;

namespace Starship.Language.Lyrics.Models {
    public class LyricLineModel {
        public LyricLineModel() {
            Words = new List<WordModel>();
        }

        public List<WordModel> Words { get; set; }

        public int Syllables {
            get { return Words.Sum(each => each.Syllables); }
        }

        public int Index { get; set; }
    }
}