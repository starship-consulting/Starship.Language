using System;
using System.Collections.Generic;
using System.Linq;
using Starship.Core.Extensions;
using Starship.Language.Resources;

namespace Starship.Language.Phonetics {
    public static class ArticulationMapper {

        static ArticulationMapper() {
            Manners = new Dictionary<string, ArticulationManners>();
            Places = new Dictionary<string, ArticulationPlaces>();
            SubSets = new Dictionary<string, int>();

            foreach (var line in EnglishResources.phonemes.ReadLines()) {
                var split = line.Split(new[] {' '}, StringSplitOptions.RemoveEmptyEntries).SelectMany(each => each.Split(new[] {'\t'}, StringSplitOptions.RemoveEmptyEntries)).ToArray();
                var key = split[0].ToLower();

                Manners.Add(key, (ArticulationManners)Enum.Parse(typeof(ArticulationManners), split[1], true));
                Places.Add(key, (ArticulationPlaces)Enum.Parse(typeof(ArticulationPlaces), split[2], true));
                SubSets.Add(key, int.Parse(split[3]));
            }
        }

        public static Dictionary<string, ArticulationManners> Manners { get; set; }

        public static Dictionary<string, ArticulationPlaces> Places { get; set; }

        public static Dictionary<string, int> SubSets { get; set; }
    }
}