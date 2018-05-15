using System;
using CrowdCode.Library.Modules.Language.NGrams;
using Starship.Language;

namespace CrowdCode.Library.Modules.Language {
    public class OrderedWord {

        public OrderedWord(NGram ngram, string word, int index) {
            NGram = ngram;
            Index = index;

            Word = English.GetWord(word);

            if (Word != null) {
                Word.NGrams.Add(this);
            }
        }

        public NGram NGram { get; set; }

        public Word Word { get; set; }

        public int Index { get; set; }

        public override string ToString() {
            return Word.ToString();
        }
    }
}