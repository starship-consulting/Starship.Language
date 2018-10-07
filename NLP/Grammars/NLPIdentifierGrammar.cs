using System;
using System.Collections.Generic;

namespace Starship.Language.NLP.Grammars {
    public class NLPIdentifierGrammar : NLPGrammar {
        public override NLPTokenTypes IdentifySymbol(NLPLexicalContext state) {
            var symbol = state.CurrentSymbol.ToLower();
            
            if(Symbols.Contains(symbol)) {
                return NLPTokenTypes.Identifier;
            }

            return NLPTokenTypes.Unknown;
        }

        private static readonly List<string> Symbols = new List<string> {
            "a", "an"
        };
    }
}