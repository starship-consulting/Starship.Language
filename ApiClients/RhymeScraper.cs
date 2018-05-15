using System.Net.Http;
using System.Threading.Tasks;

namespace Starship.Language.ApiClients {
    public class RhymeScraper {
        public RhymeScraper(HttpClient client) {
            Client = client;
        }

        public async Task<string> Scrape(string word) {
            return await Client.GetStringAsync("https://wordsapiv1.p.mashape.com/words/" + word + "/rhymes");
        }

        public HttpClient Client { get; set; }
    }
}