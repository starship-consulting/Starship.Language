using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Starship.Core.Extensions;

namespace Starship.Language.ApiClients.Datamuse {
    public class DatamuseApiClient {

        public DatamuseApiClient() {
            Client = new HttpClient();
        }

        public DatamuseApiClient(HttpClient client) {
            Client = client;
        }

        public List<DatamuseResult> Query(DatamuseQuery query) {
            var queryString = query.ToQueryString();
            return GetResults("http://api.datamuse.com/words?" + queryString);
        }

        public List<DatamuseResult> Means(string text) {
            return Query(new DatamuseQuery {
                MeansLike = text
            });
        }

        public RelatedWords GetRelatedWords(string word) {
            var words = new RelatedWords();

            words.Means = (GetResults($"http://api.datamuse.com/words?ml={word}")).OrderByDescending(each => each.Score).ToList();
            words.Synonyms = (GetResults($"http://api.datamuse.com/words?rel_syn={word}")).OrderByDescending(each => each.Score).ToList();
            words.Kinds = (GetResults($"http://api.datamuse.com/words?rel_spc={word}")).OrderByDescending(each => each.Score).ToList();
            words.Comprises = (GetResults($"http://api.datamuse.com/words?rel_com={word}")).OrderByDescending(each => each.Score).ToList();
            words.Homophones = (GetResults($"http://api.datamuse.com/words?rel_hom={word}")).OrderByDescending(each => each.Score).ToList();
            words.Consonants = (GetResults($"http://api.datamuse.com/words?rel_cns={word}")).OrderByDescending(each => each.Score).ToList();
            words.Rhymes = GetRhymes(word);

            return words;
        }

        public List<DatamuseResult> GetRhymes(string word) {
            var results = new List<DatamuseResult>();

            results.AddRange(GetResults($"http://api.datamuse.com/words?rel_rhy={word}"));
            results.AddRange(GetResults($"http://api.datamuse.com/words?rel_nry={word}"));
            results.AddRange(GetResults($"http://api.datamuse.com/words?sl={word}"));
            
            return results.OrderByDescending(each => each.Score).ToList();
        }
        
        private List<DatamuseResult> GetResults(string url) {
            var results = Client.GetStringAsync(url).GetResult();
            return JsonConvert.DeserializeObject<List<DatamuseResult>>(results);
        }

        public class DatamuseResult {
            public string Word { get; set; }

            public int Score { get; set; }

            public int NumSyllables { get; set; }

            public override string ToString() {
                return JsonConvert.SerializeObject(this);
            }
        }

        public class RelatedWords
        {
            public List<DatamuseResult> Rhymes { get; set; }
             
            public List<DatamuseResult> Means { get; set; }

            public List<DatamuseResult> Synonyms { get; set; }

            public List<DatamuseResult> Kinds { get; set; }

            public List<DatamuseResult> Comprises { get; set; }

            public List<DatamuseResult> Homophones { get; set; }

            public List<DatamuseResult> Consonants { get; set; } 
            
            public string Word { get; set; }
        }

        public HttpClient Client { get; set; }
    }
}