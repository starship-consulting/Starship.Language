using System;
using System.Linq;

namespace Starship.Language.NLP.Grammars {
    public class NLPIntegerGrammar : NLPGrammar {

        public override NLPTokenTypes IdentifySymbol(NLPLexicalContext state) {
            if(state.CurrentSymbol.All(char.IsNumber)) {
                return NLPTokenTypes.Integer;
            }

            return NLPTokenTypes.Unknown;
        }
    }
}