using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;
using Starship.Azure.Data;
using Starship.Core.Extensions;

namespace Starship.Language.Lyrics.Models {

    [Table("Word")]
    public class WordEntity : BaseEntity {

        public WordData ToData() {
            var rawData = JsonConvert.DeserializeObject<RawWordData>(Data);

            var data = new WordData(rawData);

            if (!Rhymes.IsEmpty()) {
                data.Rhymes = JsonConvert.DeserializeObject<List<string>>(Rhymes);
            }

            return data;
        }

        public string Text { get; set; }

        public string Data { get; set; }

        public string Rhymes { get; set; }
    }
}