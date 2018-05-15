using System;
using System.Collections.Generic;
using System.Linq;
using MathNet.Spatial.Euclidean;
using Starship.Core.Extensions;

namespace Starship.Language.Phonetics {
    public class PhonemeDefinition {
        static PhonemeDefinition() {
            Definitions = new Dictionary<string, PhonemeDefinition>();
        }

        protected PhonemeDefinition(string text) {
            Id = text;
            Text = text;
            TeethDistance = -1;
            TonguePosition = new Point2D(-1, -1);

            if (ArticulationPlace == ArticulationPlaces.PostAlveolar || ArticulationPlace == ArticulationPlaces.Interdental) {
                TeethDistance = 0;
                IsDental = true;
            }

            if (ArticulationPlace == ArticulationPlaces.Interdental) {
                TonguePosition = new Point2D(0, 2);
            }
            else if (ArticulationPlace == ArticulationPlaces.Alveolar) {
                if (ArticulationManner == ArticulationManners.Nasal) {
                    TonguePosition = new Point2D(1, 0);
                }
                else {
                    TonguePosition = new Point2D(3, 1);
                }
            }

            switch (ArticulationManner) {
                case ArticulationManners.Vowel:
                    Sonority = 10;
                    break;
                case ArticulationManners.Semivowel:
                    Sonority = 9;
                    break;
                case ArticulationManners.Liquid:
                case ArticulationManners.Approximant:
                    Sonority = 8;
                    break;
                case ArticulationManners.Nasal:
                    Sonority = 6;
                    break;
                case ArticulationManners.Aspirate:
                case ArticulationManners.Fricative:
                    Sonority = 4;
                    break;
                case ArticulationManners.Affricate:
                    Sonority = 2;
                    break;
                default:
                    Sonority = 0;
                    break;
            }
            
            switch (Text) {
                // Consonants

                /*case "d":
                case "s":
                    TonguePosition = new Point2D(1, 0);
                    break;*/
                    
                case "ch":
                    TonguePosition = new Point2D(6, 0);
                    break;

                case "m":
                    TonguePosition = new Point2D(1, 2);
                    break;

                case "l":
                    TonguePosition = new Point2D(0, 0);
                    break;

                // Monophthongs
                case "iy":
                    TonguePosition = new Point2D(0, 0);
                    break;
                case "ih":
                    TonguePosition = new Point2D(1, 0);
                    break;
                case "uw":
                    TonguePosition = new Point2D(5, 0);
                    break;
                case "uh":
                    TonguePosition = new Point2D(6, 0);
                    break;
                case "eh":
                    TonguePosition = new Point2D(1, 1);
                    break;
                case "er":
                    TonguePosition = new Point2D(3, 1);
                    break;
                case "ah":
                    TonguePosition = new Point2D(4.5, 1.5); // Tricky?
                    break;
                case "ao":
                    TonguePosition = new Point2D(5, 1);
                    break;
                case "ae":
                    TonguePosition = new Point2D(1, 2);
                    break;
                case "aa":
                    TonguePosition = new Point2D(1, 4);
                    break;

                // Diphthongs (always longer to verbalize? Determine distance between each point? Maybe use TongueStartPosition and TongueEndPosition)
                case "aw": //au
                    TonguePosition = new Point2D(4, 1);
                    break;
                case "ay": //ai
                    TonguePosition = new Point2D(3, 1);
                    break;
                case "ey": //ei
                    TeethDistance = 3;
                    TonguePosition = new Point2D(1.5, 0.5);
                    break;
                case "ow": //ou
                    TonguePosition = new Point2D(4, 0);
                    break;
                case "oy": //ci
                    TonguePosition = new Point2D(2, 0.5);
                    break;

                //lingua-palatal
                case "sh": // ∫
                    TonguePosition = new Point2D(6, 0);
                    break;

                //lingua-palatal
                case "zh": // ʒ
                    IsVoiced = true;
                    break;
            }
        }

        public static PhonemeDefinition Get(string text) {
            text = text.ToLower();

            if (text.Last().IsNumeric()) {
                //Stress = int.Parse(text.Last().ToString());
                text = text.Substring(0, text.Length - 1);
                //Id = Text + Stress;
            }
            else {
                //Stress = -1;
                //Id = Text;
            }

            lock (Definitions) {
                if (!Definitions.ContainsKey(text)) {
                    Definitions.Add(text, new PhonemeDefinition(text));
                }

                return Definitions[text];
            }
        }

        public double GetTransitionTime(PhonemeDefinition next) {

            if (ArticulationPlace == ArticulationPlaces.Palatal) {
                return 3;
            }

            if (ArticulationManner == ArticulationManners.Liquid) {
                return 3;
            }

            if (ArticulationManner == ArticulationManners.Vowel && next.ArticulationManner == ArticulationManners.Stop
                && (next.ArticulationPlace == ArticulationPlaces.Bilabial || next.ArticulationPlace == ArticulationPlaces.Velar)) {
                return 3;
            }

            if (/*next.ArticulationManner == ArticulationManners.Stop ||*/ next.ArticulationManner == ArticulationManners.Aspirate) {
                return 3;
            }

            if (next.TeethDistance >= 0 && TeethDistance >= 0) {
                var teethDistance = Math.Abs(TeethDistance - next.TeethDistance);

                if (teethDistance >= 3) {
                    return teethDistance;
                }
            }

            if (TonguePosition.X < 0 || TonguePosition.Y < 0 || next.TonguePosition.X < 0 || next.TonguePosition.Y < 0) {
                return 1;
            }

            if (ArticulationManner != ArticulationManners.Vowel && next.ArticulationManner == ArticulationManners.Vowel) {
                return 3;
            }

            if (next.ArticulationPlace == ArticulationPlaces.PostAlveolar) {
                return 2;
            }

            if (ArticulationPlace == ArticulationPlaces.PostAlveolar) {
                return 4;
            }
            /*if (phoneme.ArticulationManner == ArticulationManners.Affricate && phoneme.ArticulationPlace == ArticulationPlaces.PostAlveolar) {
                return 2;
            }

            if (ArticulationPlace == ArticulationPlaces.PostAlveolar) {
                return 3;
            }*/

            var distance = TonguePosition.DistanceTo(next.TonguePosition);

            return distance;
            /*foreach (var distance in ArticulationDistance) {
                if (ArticulationPlace == distance.Item1 || ArticulationManner == distance.Item2) {
                    if (phoneme.ArticulationPlace == distance.Item1 || phoneme.ArticulationManner == distance.Item2) {
                        return distance.Item3;
                    }
                }
            }*/

            /*foreach (var distance in Distance) {
                if (Text == distance.Item1 || Text == distance.Item2) {
                    if (phoneme.Text == distance.Item1 || phoneme.Text == distance.Item2) {
                        return distance.Item3;
                    }
                }
            }*/

            //return 0;
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

        public int Sonority { get; set; }

        public Point2D TonguePosition { get; set; }

        public int Stress { get; set; }

        public string Text { get; set; }

        public string Id { get; set; }

        public int TeethDistance { get; set; }
        
        public bool IsDental { get; set; }

        public bool IsVoiced { get; set; }

        private static Dictionary<string, PhonemeDefinition> Definitions { get; set; }
    }
}