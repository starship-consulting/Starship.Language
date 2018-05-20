using System.Collections.Generic;
using System.Linq;
using Starship.Language.Syllables;

namespace Starship.Language {
    public class ColoredWordGroup {
        public ColoredWordGroup() {
            Texts = new List<ColoredText>();
        }

        public static List<ColoredWordGroup> Get(Phrase source, IEnumerable<Phrase> rhymes) {
            var results = new List<ColoredWordGroup>();
            var patterns = new List<RhymePattern>();

            foreach (var syllable in source.Words.SelectMany(each => each.Syllables)) {
                var pattern = patterns.FirstOrDefault(each => each.Syllable.RhymesWith(syllable));

                if (pattern == null) {
                    patterns.Add(new RhymePattern {
                        Index = patterns.Count() + 1,
                        Syllable = syllable
                    });
                }
            }

            foreach (var rhyme in rhymes) {
                var wordGroup = new ColoredWordGroup { Word = rhyme.Text };
                results.Add(wordGroup);

                foreach (var word in rhyme.Words) {
                    ColoredText currentText = null;

                    foreach (var syllable in word.Syllables) {
                        var pattern = patterns.FirstOrDefault(each => each.Syllable.RhymesWith(syllable));

                        currentText = new ColoredText {
                            Text = syllable.Text
                        };

                        if (pattern != null) {
                            currentText.Index = pattern.Index;
                        }
                        
                        wordGroup.Texts.Add(currentText);
                    }

                    if (currentText != null) {
                        currentText.Text += " ";
                    }
                }
            }

            return results;
        }

        public class RhymePattern {
            public Syllable Syllable { get; set; }

            public int Index { get; set; }
        }

        public string Word { get; set; }

        public List<ColoredText> Texts { get; set; }
    }
}