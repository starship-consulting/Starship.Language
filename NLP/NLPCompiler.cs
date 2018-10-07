using System;
using System.Collections.Generic;
using Starship.Language.NLP.Grammars;

namespace Starship.Language.NLP {
    public static class NLPCompiler {
        
        public static void Run(string code) {

            var lexers = new List<NLPGrammar> {
                new NLPAssignmentGrammar(),
                new NLPIdentifierGrammar(),
                new NLPIntegerGrammar(),
                new NLPNounGrammar()
            };

            var analyzer = new NLPAnalyzer(lexers);
            var tokens = analyzer.Tokenize(code);
            var parser = new NLPParser();
            parser.Parse(tokens.ToArray());
        }
    }
}