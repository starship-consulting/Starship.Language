using System.Net.Http;
using Newtonsoft.Json;
using Starship.Core.Extensions;
using Starship.Language.Lyrics.Models;

namespace Starship.Language.ApiClients {
    public class WordsApiClient {

        public WordsApiClient() {
            Client = new HttpClient();
            Client.DefaultRequestHeaders.Add("X-Mashape-Key", "");
        }

        public WordsApiClient(HttpClient client) {
            Client = client;
        }

        public RawWordData GetRaw(string word) {
            var result = Client.GetStringAsync("https://wordsapiv1.p.mashape.com/words/" + word).GetResult();
            return JsonConvert.DeserializeObject<RawWordData>(result);
        }

        public string GetString(string word) {
            return Client.GetStringAsync("https://wordsapiv1.p.mashape.com/words/" + word).GetResult();
        }

        public object Get(string word) {
            return JsonConvert.DeserializeObject(Client.GetStringAsync("https://wordsapiv1.p.mashape.com/words/" + word).GetResult());
        }

        public HttpClient Client { get; set; }
    }
}