using System.Collections.Generic;
using System.Linq;
using Starship.Core.Collections;

namespace Starship.Language.Phonetics {
    public class Phoneme {
        public Phoneme() {
            Letters = string.Empty;
        }

        public Phoneme(string text) : this() {
            Definition = PhonemeDefinition.Get(text);

            /*Text = text.ToLower();

            if (Text.Last().IsNumeric()) {
                Stress = int.Parse(Text.Last().ToString());
                Text = Text.Substring(0, Text.Length - 1);
                Id = Text + Stress;
            }
            else {
                Stress = -1;
                Id = Text;
            }*/
        }

        public static List<Phoneme> Parse(string phonemes) {
            return phonemes.Split(' ').Select(each => new Phoneme(each)).ToList();
        }

        public TupleList<ArticulationPlaces, ArticulationManners, float> ArticulationDistance = new TupleList<ArticulationPlaces, ArticulationManners, float> {
            {ArticulationPlaces.PostAlveolar, ArticulationManners.Vowel, 0.5f}
        };
        
        public double GetTransitionTime(Phoneme next) {
            if (next.IsSilent()) {
                return 0;
            }

            return Definition.GetTransitionTime(next.Definition);
        }

        public bool SoundsLike(Phoneme phoneme) {
            if (Id == phoneme.Id) {
                return true;
            }

            var articulationMatch = phoneme.ArticulationManner == ArticulationManner || (phoneme.HasVowel() && HasVowel());

            if (articulationMatch) {
                if (phoneme.ArticulationManner == ArticulationManners.Nasal) {
                    return true;
                }

                if (phoneme.ArticulationSubType == ArticulationSubType && ArticulationSubType > 0) {
                    return true;
                }
            }

            if (Text == phoneme.Text && Stress > 0 && phoneme.Stress > 0) {
                return true;
            }

            var comparer = new Core.Utility.ObjectComparer<Phoneme>(this, phoneme);

            if (comparer.Compare((p1, p2) => p1.ArticulationManner == ArticulationManners.Fricative && p1.ArticulationPlace == ArticulationPlaces.Alveolar && p2.ArticulationManner == ArticulationManners.Stop && p2.ArticulationPlace == ArticulationPlaces.Alveolar)) {
                return true;
            }
            
            return false;
        }

        public bool IsSilent() {
            if (Text == "ah" && Letters == "t") {
                return true;
            }

            if (Text == "s") {
                return true;
            }

            //if (previous.ArticulationManner == ArticulationManners.Fricative && ArticulationManner == ArticulationManners.Stop) {
            //    return true;
            //}

            /*if (!previous.HasVowel() && Text == "t") {
                return true;
            }

            if (previous.Text == "ng" && Text == "k") {
                return true;
            }

            if (Text == "s") {
                return true;
            }*/

            return false;
        }

        public bool IsSemiVowel() {
            return ArticulationManner == ArticulationManners.Semivowel;
        }

        public bool IsVowel() {
            return ArticulationManner == ArticulationManners.Vowel;
        }

        public bool HasVowel() {
            return ArticulationManner == ArticulationManners.Vowel || ArticulationManner == ArticulationManners.Semivowel;
        }

        private static readonly KeyValueList<string, string> PerfectPatterns = new KeyValueList<string, string> {
            {"ih", "ee"},
            {"f", "ph"},
            {"iy", "ei"},
            {"ch", "tch"},
            {"eh", "ie"},
            {"hh", "h"},
            {"t", "tt"},
            {"zh", "si"}
        };

        public bool IsDefiniteLetter(string letter) {
            if (Text == "er" && Letters.Any() && !Letters.Contains("r")) {
                return true;
            }

            foreach (var pattern in PerfectPatterns) {
                if (pattern.Key == Text) {
                    if (Letters + letter == pattern.Value) {
                        return true;
                    }
                }
            }

            return Letters + letter == Text;
        }

        public string[] GetPossibleLetters() {
            List<string> matches = Text.Select(each => each.ToString()).ToList();

            if (Letters.Any()) {
                //matches = Text.Select(each => each.ToString()).ToList();
            }
            else {
                //matches = Text.Take(1).Select(each => each.ToString()).ToList();
            }

            // Todo:  Use patterns like this instead?
            if (ArticulationPlace == ArticulationPlaces.Glottal) {
                matches.AddRange(new[] {"h"});
            }

            switch (Text) {
                case "aa":
                    matches.AddRange(new[] {"o", "h", "r", "u"});
                    break;
                case "ae":
                    matches.AddRange(new[] {"i"});
                    break;
                case "ah":
                    matches.AddRange(new[] {"i", "l", "o", "e", "t", "y", "u"}); // bustling (silent t)
                    break;
                case "ao":
                    matches.AddRange(new[] {"u", "h"});
                    break;
                case "aw":
                    return new[] {"o", "u", "g", "h", "w"};
                case "ay":
                    return new[] {"e", "u", "y", "i", "g", "h"};
                case "b":
                    matches.AddRange(new[] {"a", "i"});
                    break;
                case "ch":
                    matches.AddRange(new[] {"t"});
                    break;
                case "dh":
                    matches.AddRange(new[] {"t"});
                    break;
                case "eh":
                    matches.AddRange(new[] {"a", "i", "y"});
                    break;
                case "ey":
                    matches.AddRange(new[] {"a", "e", "i", "g", "h"});
                    break;
                case "er":
                    matches.AddRange(new[] { "a" });
                    break;
                case "f":
                    matches.AddRange(new[] {"p", "h"});
                    break;
                case "g":
                    matches.AddRange(new[] {"x"});
                    break;
                case "k":
                    /*if (Letters.Contains("q")) {
                        matches.AddRange(new[] {"u", "i"});
                    }*/

                    matches.AddRange(new[] {"c", "x", "h", "q"});
                    break;
                case "l":
                    matches.AddRange(new[] {"a"});
                    break;
                case "iy":
                    matches.AddRange(new[] {"e", "a"});
                    break;
                case "jh":
                    matches.AddRange(new[] {"d", "g"});
                    break;
                case "ih":
                    matches.AddRange(new[] {"e", "y", "a"});
                    break;
                case "m":
                    matches.AddRange(new[] {"n"});
                    break;
                case "n":
                    matches.AddRange(new[] {"e", "d"});
                    break;
                case "ow":
                    matches.AddRange(new[] {"h", "a"});
                    break;
                case "oy":
                    matches.AddRange(new[] {"i", "u", "e"});
                    break;
                case "p":
                    matches.AddRange(new[] {"e", "i", "a"});
                    break;
                case "r":
                    matches.AddRange(new[] {"e"});
                    break;
                case "s":
                    matches.AddRange(new[] {"c", "z" /*, "t"*/});
                    break;
                case "sh":
                    matches.AddRange(new[] {"t", "i", "c", "e"});
                    break;
                case "t":
                    matches.AddRange(new[] {"d", "e"});
                    break;
                case "uh":
                    matches.AddRange(new[] {"o"});
                    break;
                case "uw":
                    matches.AddRange(new[] {"h", "i"});
                    break;
                case "v":
                    matches.AddRange(new[] {"s"});
                    break;
                case "w":
                    matches.AddRange(new[] {"h"});
                    break;
                case "y":
                    matches.AddRange(new[] {"j", "l"});
                    break;
                case "z":
                    matches.AddRange(new[] {"s", "e", "i"});
                    break;
                case "zh":
                    matches.AddRange(new[] {"s", "g", "i", "j"});
                    break;
            }

            return matches.ToArray();
        }

        public int ArticulationSubType {
            get { return ArticulationMapper.SubSets[Text]; }
        }

        public ArticulationPlaces ArticulationPlace {
            get { return ArticulationMapper.Places[Text]; }
        }

        public ArticulationManners ArticulationManner {
            get { return ArticulationMapper.Manners[Text]; }
        }
        
        public string Id => Definition.Id;

        public int Stress => Definition.Stress;

        public string Text => Definition.Text;

        public PhonemeDefinition Definition { get; set; }

        public string Reason { get; set; }

        public string Letters { get; set; }

        public string Description {
            get {
                var description = string.Empty;

                if (Definition.IsVoiced) {
                    description += "voiced ";
                }

                description += "'" + Letters + "'";

                return description.Trim();
            }
        }

        public override string ToString() {
            return Id;
        }
    }
}