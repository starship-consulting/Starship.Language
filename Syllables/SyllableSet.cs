using System.Collections.Generic;
using Starship.Language.Syllables;

namespace CrowdCode.Library.Modules.Language.Syllables {
    public class SyllableSet {
        public SyllableSet() {
            Syllables = new List<Syllable>();
        }

        public List<Syllable> Syllables { get; set; }
    }
}