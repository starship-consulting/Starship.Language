using Newtonsoft.Json;

namespace Starship.Language.ApiClients.Datamuse {
    public class DatamuseQuery {

        [JsonProperty(PropertyName = "rel_rhy")]
        public string RhymesWith { get; set; }

        [JsonProperty(PropertyName = "ml")]
        public string MeansLike { get; set; }

        [JsonProperty(PropertyName = "sl")]
        public string SoundsLike { get; set; }

        [JsonProperty(PropertyName = "rel_syn")]
        public string SynonymOf { get; set; }

        [JsonProperty(PropertyName = "rel_nry")]
        public string ApproximateRhyme { get; set; }

        [JsonProperty(PropertyName = "rel_bga")]
        public string ComesAfter { get; set; }

        [JsonProperty(PropertyName = "rel_bgb")]
        public string ComesBefore { get; set; }

        [JsonIgnore]
        public int MaxSyllables { get; set; }
    }
}