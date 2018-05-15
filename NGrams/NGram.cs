using System.Collections.Generic;
using System.Linq;

namespace CrowdCode.Library.Modules.Language.NGrams {
    public class NGram {
        
        public NGram(string text, string[] words, long frequency) {
            Text = text;
            Frequency = frequency;
            Words = words.Select((each, i) => new OrderedWord(this, each, i + 1)).ToList();
        }
        
        public long Frequency { get; set; }

        public string Text { get; set; }

        public List<OrderedWord> Words { get; set; }

        public override string ToString() {
            return Text;
        }
    }
}