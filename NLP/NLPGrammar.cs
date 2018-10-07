using System;

namespace Starship.Language.NLP {
    public abstract class NLPGrammar {

        public abstract NLPTokenTypes IdentifySymbol(NLPLexicalContext state);
    }
}