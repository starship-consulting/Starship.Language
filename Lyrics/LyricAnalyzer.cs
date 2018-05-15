using System.Collections.Generic;
using System.Linq;
using Starship.Language.Lyrics.Models;

namespace Starship.Language.Lyrics {
    public class LyricAnalyzer {

        public LyricAnalyzer() {
            ParsedTexts = new List<ParsedText>();
            RhymeGroups = new List<RhymeGroup>();
        }
        
        public LyricModel Analyze(string text) {
            var parser = new WordParser();
            var model = new LyricModel();

            ParsedTexts = parser.Parse(text);
            RhymeGroups = new RhymeBuilder().Apply(ParsedTexts.ToArray()).RhymeGroups.ToList();
            
            model.Lines = ParsedTexts.GroupBy(each => each.Line)
                .Select((group, index) => new LyricLineModel {
                    Index = index,
                    Words = group.Select(word => new WordModel(word)).ToList()
                })
                .ToList();

            return model;
        }
        
        public List<ParsedText> ParsedTexts { get; set; }

        public List<RhymeGroup> RhymeGroups { get; set; } 
    }
}