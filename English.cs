using System;
using System.Collections.Generic;
using System.Linq;
using Starship.Core.Extensions;
using Starship.Language.NGrams;
using Starship.Language.Phonetics;
using Starship.Language.Resources;

namespace Starship.Language {
    public class English {
        static English() {
            Vocabulary = new Dictionary<string, Word>();

            //var pronounce = EnglishResources.pronounce.Replace("\t", " ").ToLower();
            //File.WriteAllText(@"C:\Users\Josh\Downloads\pronounce.txt", pronounce);

            var words = EnglishResources.pronounce.Replace("\t", " ").ToLower().ReadLines();

            /*var punctuated = new List<string>();

            foreach (var word in words) {
                if (word.Contains("'") && !word.Contains("'s"))) {
                    punctuated.Add(word);
                }
            }*/

            var tokens = IllegalTokens;

            var results = words.ForEachParallel(line => {
                if (tokens.Any(line.Contains)) {
                    return null;
                }

                var segments = line.Replace("'", "").Replace("-", "").Split(new[] {" "}, StringSplitOptions.RemoveEmptyEntries);
                var word = segments.First().Trim();
                var hasNumbers = word.HasNumbers();

                if (!word[0].IsAlphabetical() || (hasNumbers && !word.Contains("(")) || !word.HasVowel()) {
                    return null;
                }

                if (hasNumbers) {
                    word = word.Split('(')[0];
                }

                var phonemes = segments.Skip(1).Select(each => new Phoneme(each)).ToList();

                var resolver = new PhonemeResolver(word);
                resolver.BindPhonemes(phonemes.ToArray());

                return new Word(word, phonemes);
            });

            foreach (var word in results) {
                if (!Vocabulary.ContainsKey(word.Text)) {
                    Vocabulary.Add(word.Text, word);
                }
                else {
                    Vocabulary[word.Text].Homographs.Add(word);
                }
            }

            RhymeFinder.BuildRhymes();
        }
        
        public static Word GetWord(string text) {
            return Vocabulary.ContainsKey(text) ? Vocabulary[text] : null;
        }

        public static List<Word> GetWords() {
            return Vocabulary.Values.ToList();
        }

        public static List<NGramTree> GetNGrams() {
            InitializeNGrams();
            return NGrams.Values.ToList();
        }

        public static NGramTree GetNGrams(int length) {
            InitializeNGrams();
            return NGrams.ContainsKey(length) ? NGrams[length] : null;
        }

        public static void InitializeNGrams() {
            if (NGrams == null) {
                var builder = new NGramBuilder();
                NGrams = builder.Load();
            }
        }

        public static readonly string[] IllegalTokens = {".", "'s"};
        
        private static NGramCollection NGrams { get; set; }

        private static Dictionary<string, Word> Vocabulary { get; set; }
    }
}