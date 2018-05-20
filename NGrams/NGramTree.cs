using System.Collections.Generic;

namespace Starship.Language.NGrams {
    public class NGramTree : Dictionary<string, NGram> {

        public NGramTree(int length) {
            NGramLength = length;
        }

        public int NGramLength { get; set; }
    }
}