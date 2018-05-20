using System.Collections.Generic;
using System.Linq;
using Starship.Core.Extensions;

namespace Starship.Language.Rhymes {
    public class RhymeGenerator {
        public RhymeGenerator(string word) {
            Word = English.GetWord(word);
        }

        public List<Word> GetRhymes() {
            var results = new List<Word>();
            var vowels = Word.Text.Count(each => each.IsVowel());

            if (vowels <= 1) {
            }

            return results;
        }

        private Word Word { get; set; }
    }
}