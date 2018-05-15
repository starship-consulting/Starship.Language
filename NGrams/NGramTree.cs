using System.Collections.Generic;

namespace CrowdCode.Library.Modules.Language.NGrams {
    public class NGramTree : Dictionary<string, NGram> {

        public NGramTree(int length) {
            NGramLength = length;
        }

        public int NGramLength { get; set; }
    }
}