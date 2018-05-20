using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Starship.Azure.Data;
using Starship.Azure.Extensions;
using Starship.Core.Extensions;
using Starship.Language.ApiClients;
using Starship.Language.Lyrics.Models;

namespace Starship.Language.Lyrics {
    public class WordParser {

        public List<ParsedText> Parse(string text) {
            var results = new List<ParsedText>();

            // Line segmenting
            var lines = text.Split('\n');
            var lineNumber = 0;

            foreach (var line in lines) {
                lineNumber += 1;

                var segments = line.Split(' ');
                var index = 1;

                foreach (var segment in segments)
                {
                    var cleanedText = RemoveInvalidCharacters(segment).Trim().ToLower();

                    if (cleanedText.IsEmpty())
                    {
                        continue;
                    }

                    var model = new ParsedText
                    {
                        Index = index,
                        Text = segment,
                        CleanedText = cleanedText,
                        Line = lineNumber
                    };

                    results.Add(model);

                    index += 1;
                }

                var uniqueWords = results.GroupBy(each => each.CleanedText).Select(each => each.Key);
                var words = GetWords(uniqueWords.ToArray());

                foreach (var word in words)
                {
                    var matches = results.Where(each => each.CleanedText == word.Word);

                    foreach (var match in matches)
                    {
                        match.Word = word;
                    }
                }
            }

            return results;
        }

        public List<WordData> GetWords(params string[] words) {
            var results = new List<WordData>();

            var context = new DatabaseContext();
            var entities = context.Set<WordEntity>().Where(each => words.Contains(each.Text)).ToList();

            foreach (var word in words) {
                var entity = entities.FirstOrDefault(each => each.Text == word);

                if (entity == null) {
                    entity = context.Add(new WordEntity {
                        Text = word
                    });

                    try {
                        entity.Data = new WordsApiClient().GetString(word);
                        entity.Rhymes = JsonConvert.SerializeObject(new PrimeRhymeScraper().Scrape(word));
                    }
                    catch (Exception) {
                        entity.Data = "{ 'word': '" + word + "', 'unknown': true }";
                    }
                }

                results.Add(entity.ToData());
            }

            context.SaveChanges();

            return results;
        }

        private string RemoveInvalidCharacters(string text) {
            Regex rgx = new Regex("[^a-zA-Z0-9 -]");
            return rgx.Replace(text, "");
        }
    }
}