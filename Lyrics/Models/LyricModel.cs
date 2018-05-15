using System.Collections.Generic;
using System.Linq;

namespace Starship.Language.Lyrics.Models {
    public class LyricModel {

        public LyricModel() {
            Lines = new List<LyricLineModel>();
        }

        public List<LyricLineModel> Lines { get; set; }

        public int Syllables {  get { return Lines.Sum(each => each.Syllables); } }
    }
}