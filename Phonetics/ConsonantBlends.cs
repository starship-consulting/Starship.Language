using System.Collections.Generic;
using CrowdCode.Library.Modules.Language.Resources;
using Starship.Core.Extensions;

namespace Starship.Language.Phonetics {
    public static class ConsonantBlends {

        static ConsonantBlends() {
            Blends = new List<string>();

            foreach (var line in EnglishResources.blends.ReadLines()) {
                var text = line.ToLower().Trim();

                if (!Blends.Contains(text)) {
                    Blends.Add(text);
                }
            }
        }

        public static List<string> Blends { get; set; }
    }
}