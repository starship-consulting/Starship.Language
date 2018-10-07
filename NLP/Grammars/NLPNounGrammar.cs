namespace Starship.Language.NLP.Grammars {
    public class NLPNounGrammar : NLPGrammar {
        public override NLPTokenTypes IdentifySymbol(NLPLexicalContext state) {
            return NLPTokenTypes.Noun;
        }
    }
}