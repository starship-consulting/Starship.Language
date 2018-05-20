using System.Collections.Generic;
using System.Linq;
using Starship.Core.Collections;
using Starship.Core.Extensions;
using Starship.Language.Resources;

namespace Starship.Language.NGrams {
    public class NGramBuilder {

        public class NGramItem {
            public long Frequency { get; set; }

            public string Text { get; set; }

            public string[] Words { get; set; }
        }

        public NGramCollection Load(bool useFiltered = true) {
            var collection = new NGramCollection();

            for (var count = 1; count <= 6; count++) {
                collection.Add(count, new NGramTree(count));
            }

            UseFiltered = useFiltered;

            if (UseFiltered) {
                var ngrams = EnglishResources.ngrams_filtered.ReadLines().ForEachParallel(GetNGrams);

                foreach (var gram in ngrams) {
                    collection[gram.Words.Length].Add(gram.Text, new NGram(gram.Text, gram.Words, gram.Frequency));
                }
            }
            else {
                /*var lines = EnglishResources.ngrams1.ReadLines()
                    .Concat(EnglishResources.ngrams2.ReadLines())
                    .Concat(EnglishResources.ngrams3.ReadLines())
                    .Concat(EnglishResources.ngrams4.ReadLines())
                    .Concat(EnglishResources.ngrams5.ReadLines());*/
            }

            return collection;
        }

        /*private void LoadNGrams(string text) {
            if (!UseFiltered) {
                text = text.Replace("-", " ");

                if (text.Contains("'")) {
                    var permutations = GetContractionPermutations(text);

                    if (permutations.Any()) {
                        foreach (var permutation in permutations) {
                            SaveLine(permutation);
                        }
                    }
                    else {
                        Dirty.Add(text);
                    }

                    return;
                }
            }

            SaveLine(text);
        }*/

        private List<string> GetContractionPermutations(string text) {
            var permutations = new List<string>();

            foreach (var contraction in Contractions) {
                permutations.AddRange(GetContractionPermutations(text, contraction.Item1, contraction.Item2, contraction.Item3, contraction.Item4));
                permutations.AddRange(GetContractionPermutations(text, contraction.Item1.Replace(" ", ""), contraction.Item2, contraction.Item3, contraction.Item4));
            }

            var finalResults = new List<string>();

            foreach (var permutation in permutations) {
                var results = GetContractionPermutations(permutation);

                if (results.Any()) {
                    finalResults.AddRange(results);
                }
                else {
                    finalResults.Add(permutation);
                }
            }

            if (!finalResults.Any()) {
                finalResults.Add(text);
            }

            return finalResults;
        }

        private List<string> GetContractionPermutations(string text, string pattern1, string pattern2, string pattern3, string pattern4) {
            var permutations = new List<string>();

            if (text.Contains(pattern1)) {
                permutations.Add(text.Replace(pattern1, pattern2));

                if (!pattern3.IsEmpty()) {
                    permutations.Add(text.Replace(pattern1, pattern3));
                }

                if (!pattern4.IsEmpty()) {
                    permutations.Add(text.Replace(pattern1, pattern4));
                }
            }

            return permutations;
        }

        private NGramItem GetNGrams(string line) {
            var segments = TextCleaner.CleanSplit(" ", line);
            var frequency = long.Parse(segments[0]);
            
            if (English.IllegalTokens.Any(line.Contains)) {
                return null;
            }

            if (frequency < MinimumFrequency) {
                return null;
            }

            var words = segments.Skip(1).ToArray();

            return new NGramItem {
                Text = string.Join(" ", words),
                Frequency = frequency,
                Words = words
            };
        }
        
        private bool UseFiltered { get; set; }

        public const int MinimumFrequency = 40;

        public List<string> Dirty = new List<string>();

        public TupleList<string, string, string, string> Contractions = new TupleList<string, string, string, string> {
            {"did n't", "didnt", "did not", ""},
            {"do n't", "dont", "do not", ""},
            {"has n't", "hasnt", "has not", ""},
            {"does n't", "doesnt", "does not", ""},
            {"ai n't", "aint", "am not", ""},
            {"is n't", "isnt", "is not", ""},
            {"ca n't", "cant", "cannot", "can not"},
            {"could n't", "couldnt", "could not", ""},
            {"was n't", "wasnt", "was not", ""},
            {"wo n't", "wont", "will not", ""},
            {"had n't", "hadnt", "had not", ""},
            {"have n't", "havent", "have not", ""},
            {"would n't", "wouldnt", "would not", ""},
            {"are n't", "arent", "are not", ""},
            {"were n't", "werent", "were not", ""},
            {"should n't", "shouldnt", "should not", ""},
            {"it's", "it is", "its", ""},
            {"need n't", "neednt", "need not", ""},
            {"he's", "he is", "hes", ""},
            {"i'm", "i am", "im", ""},
            {"she's", "shes", "she is", ""},
            {"n't ", "not", "", ""},
            {"y'all", "you all", "yall", ""},
            {"that's", "that is", "thats", ""},
            {"d' ", "do", "", ""},
            {"what's", "what is", "whats", ""},
            {"c'm", "come", "", ""},
            {"let's", "let us", "lets", ""},
            {"we're", "we are", "were", ""}
        };
    }
}