using System;
using System.Collections.Generic;
using System.Linq;
using Starship.Language.Phonetics;
using Starship.Language.Syllables;

namespace Starship.Language.Interfaces {
    public interface HasSyllables {
        List<Syllable> Syllables { get; }
    }

    public static class HasSyllablesExtensions {

        public static int GetPhoneticSpeed(this HasSyllables target) {

            if (target.Syllables.Count == 1) {
                return 1;
            }

            var incompatibleTypes = new List<Tuple<ArticulationManners, ArticulationManners>> {
                new Tuple<ArticulationManners, ArticulationManners>(ArticulationManners.Nasal, ArticulationManners.Vowel),
                new Tuple<ArticulationManners, ArticulationManners>(ArticulationManners.Nasal, ArticulationManners.Fricative),
                //new Tuple<ArticulationManners, ArticulationManners>(ArticulationManners.Liquid, ArticulationManners.Semivowel),
            };

            for (var index = 0; index < target.Syllables.Count - 1; index++) {
                var syllable = target.Syllables[index];
                var nextSyllable = target.Syllables[index + 1];

                if (syllable.Phonemes.Last().ArticulationPlace == nextSyllable.Phonemes.First().ArticulationPlace) {
                    return 99;
                }

                //{absences ([ae1 b s-ah0 n s-ih0 z)}

                //match = patterns.Any(pattern => syllable.Phonemes.Last().Type == pattern.Item1 && nextSyllable.Phonemes.First().Type == pattern.Item2);

                //if (!match) {
                //    break;
                //}
            }

            return 1;
        }
    }
}