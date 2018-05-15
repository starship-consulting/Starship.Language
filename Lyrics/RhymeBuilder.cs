using System.Collections.Generic;
using System.Linq;
using Starship.Language.Lyrics.Models;

namespace Starship.Language.Lyrics {
    public class RhymeBuilder {
        public RhymeBuilder() {
            RhymeGroups = new List<RhymeGroup>();
        }

        public RhymeBuilder Apply(params ParsedText[] texts) {
            var words = texts.Select(each => each.Word).ToArray();

            Apply(words);

            // Update the texts with their appropriate rhyme groups
            foreach (var text in texts) {
                text.RhymeGroup = FindRhymeGroup(text.CleanedText);
            }

            return this;
        }

        public RhymeBuilder Apply(params WordData[] words) {
            var distinctWords = words.GroupBy(each => each.Word).Select(each => each.First()).ToList();

            // For each word, check rhymes for all other words
            foreach (var word1 in distinctWords) {
                foreach (var word2 in distinctWords) {
                    if (!TryRhyme(word1, word2)) {
                        TryRhyme(word2, word1);
                    }
                }
            }

            return this;
        }

        private bool TryRhyme(WordData word1, WordData word2) {
            if (word1.Word == word2.Word) {
                return false;
            }

            if (word1.Rhymes.Contains(word2.Word)) {
                var group = FindRhymeGroup(word1.Word) ?? FindRhymeGroup(word2.Word) ?? NewRhymeGroup();
                group.TryAdd(word1.Word, word2.Word);
                return true;
            }

            return false;
        }

        private RhymeGroup NewRhymeGroup() {
            var group = new RhymeGroup(RhymeGroups.Count + 1);
            RhymeGroups.Add(group);
            return group;
        }

        private RhymeGroup FindRhymeGroup(string word) {
            return RhymeGroups.FirstOrDefault(each => each.Words.Contains(word));
        }

        public List<RhymeGroup> RhymeGroups { get; set; }
    }
}