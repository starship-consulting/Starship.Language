using System;
using System.Collections.Generic;
using System.Linq;
using Starship.Language.Phonetics;
using Starship.Language.Syllables.Rules;

namespace Starship.Language.Syllables {
    public class SyllableMapper {

        static SyllableMapper() {
            DefaultRules = new List<SyllableRule> {
                new SyllableMustMaintainSonority(),
                new SyllableCodaRules()
            };

            //LoadPrefixes();
            //LoadSyllables();
        }

        public SyllableMapper(List<Phoneme> phonemes) {
            Rules = DefaultRules;
            Context = new SyllableContext(phonemes);
            Word = string.Join("", phonemes.Select(each => each.Letters));
        }

        /*private static void LoadSyllables() {
            var properNouns = new List<string>();
            SyllableCache = new Dictionary<string, List<List<string>>>();

            foreach (var syllable in EnglishResources.syllables.ReadLines()) {
                // Skip proper nouns
                if (char.IsUpper(syllable, 1)) {
                    properNouns.Add(syllable);
                    continue;
                }

                var clean = syllable.Trim().ToLower();
                var word = clean.Replace("-", "");

                if (word.Contains(" ")) {
                    continue;
                }

                var syllables = clean.Split(new[] {'-'}, StringSplitOptions.RemoveEmptyEntries).ToList();

                if (!SyllableCache.ContainsKey(word)) {
                    SyllableCache.Add(word, new List<List<string>>());
                    SyllableCache[word].Add(syllables);
                }
                else {
                    if (SyllableCache[word].First().Count < syllables.Count) {
                        SyllableCache[word].Clear();
                        SyllableCache[word].Add(syllables);
                    }
                }
            }
        }*/
        
        public List<Syllable> GetSyllables() {
            do {
                var phoneme = Context.Phonemes.Current;

                if (Context.RemainingVowels() > 0) {
                    var failedRule = Rules.Select(each => new {
                        Rule = each,
                        Evaluation = each.ShouldAddPhoneme(phoneme, Context)
                    })
                    .FirstOrDefault(each => each.Evaluation.Outcome == false);

                    if (failedRule != null) {
                        var applied = ApplySyllable();
                        applied.Rule = "(" + failedRule.Rule.GetType().Name + ") " + failedRule.Evaluation.Reason;
                    }
                }

                Context.CurrentSyllable.Phonemes.Add(phoneme);
            } while (Context.Phonemes.Step());

            ApplySyllable();

            return Context.Syllables;
        }

        private Syllable ApplySyllable() {
            if (!Context.CurrentSyllable.Phonemes.Any()) {
                return Context.CurrentSyllable;
            }
            
            if (!Context.CurrentSyllable.HasNucleus()) {
                throw new Exception("Syllable without vowel.");
            }

            var applied = AddSyllable(Context.CurrentSyllable);
            Context.Syllables.Add(applied);
            Context.CurrentSyllable = new Syllable();
            return applied;
        }

        private static Syllable AddSyllable(Syllable syllable) {
            lock (UniqueSyllables) {
                if (UniqueSyllables.ContainsKey(syllable.Id)) {
                    return UniqueSyllables[syllable.Id];
                }

                UniqueSyllables.Add(syllable.Id, syllable);
                return syllable;
            }
        }

        private static List<SyllableRule> DefaultRules { get; set; }

        private List<SyllableRule> Rules { get; set; }

        private string Word { get; set; }

        private SyllableContext Context { get; set; }

        public static Dictionary<string, Syllable> UniqueSyllables = new Dictionary<string, Syllable>();

        //public static Dictionary<string, Syllable> Prefixes = new Dictionary<string, Syllable>();

        //private static Dictionary<string, List<List<string>>> SyllableCache { get; set; }
    }
}