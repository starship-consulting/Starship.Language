using System;

namespace Starship.Language.NLP {
    public struct NLPLexicalContext {

        public NLPLexicalContext(string code) {
            Code = code;
            Symbols = Code.Split(' ');
            Index = 0;
            CurrentSymbol = Symbols[Index];
            Sentences = new []{""};
        }

        public bool Advance() {
            if(Index >= Symbols.Length) {
                return false;
            }

            Index += 1;
            CurrentSymbol = Symbols[Index];
            return true;
        }

        public string Code { get; }

        public string[] Sentences { get; } // todo

        public string CurrentSymbol { get; private set; }

        public int Index { get; private set; }

        public string[] Symbols { get; }
    }
}