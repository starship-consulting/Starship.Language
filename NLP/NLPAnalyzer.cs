using System;
using System.Collections.Generic;

namespace Starship.Language.NLP {
    public class NLPAnalyzer {

        public NLPAnalyzer(List<NLPGrammar> lexers) {
            Lexers = lexers;
        }

        public List<NLPToken> Tokenize(string code) {
            var tokens = new List<NLPToken>();
            Context = new NLPLexicalContext(code);

            do {
                foreach(var lexer in Lexers) {
                    var type = lexer.IdentifySymbol(Context);

                    if(type != NLPTokenTypes.Unknown) {
                        tokens.Add(new NLPToken(type, Context.CurrentSymbol));
                        break;
                    }
                }
            }
            while(Context.Advance());

            return tokens;
        }

        private NLPLexicalContext Context { get; set; }

        private List<NLPGrammar> Lexers { get; set; }
    }
}