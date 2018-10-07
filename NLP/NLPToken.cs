using System;


namespace Starship.Language.NLP {
    public class NLPToken {

        public NLPToken(NLPTokenTypes type, string value) {
            Type = type;
            Value = value;
        }

        public NLPTokenTypes Type { get; set; }

        public string Value { get; set; }
    }
}