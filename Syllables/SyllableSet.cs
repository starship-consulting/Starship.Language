using System.Collections.Generic;

namespace Starship.Language.Syllables {
    public class SyllableSet {
        public SyllableSet() {
            Syllables = new List<Syllable>();
        }

        public List<Syllable> Syllables { get; set; }
    }
}