using System;

namespace Starship.Language.NLP.Grammars {
    public class NLPAssignmentGrammar : NLPGrammar {
        public override NLPTokenTypes IdentifySymbol(NLPLexicalContext state) {
            var symbol = state.CurrentSymbol.ToLower();
            
            if(string.Equals(symbol, "is")) {
                return NLPTokenTypes.Assignment;
            }

            return NLPTokenTypes.Unknown;
        }
    }
}