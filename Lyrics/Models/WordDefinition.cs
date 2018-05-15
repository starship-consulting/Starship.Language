using System.Collections.Generic;

namespace Starship.Language.Lyrics.Models {
    public class WordDefinition {

        public WordDefinition() {
            Synonyms = new List<string>();
            Antonyms = new List<string>();
            TypeOf = new List<string>();
            HasTypes = new List<string>();
            Examples = new List<string>();
            MemberOf = new List<string>();
            Derivation = new List<string>();
        }

        public string Definition { get; set; }

        public string PartOfSpeech { get; set; }

        public List<string> Synonyms { get; set; }

        public List<string> Antonyms { get; set; }

        public List<string> TypeOf { get; set; }

        public List<string> HasTypes { get; set; }

        public List<string> Examples { get; set; }

        public List<string> MemberOf { get; set; }

        public List<string> Derivation { get; set; }
    }
}