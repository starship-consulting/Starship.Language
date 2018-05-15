using System.Collections.Generic;
using System.Net.Http;

namespace Starship.Language.ApiClients {
    public class PrimeRhymeScraper {

        public PrimeRhymeScraper() {
            Client = new HttpClient();
        }

        public PrimeRhymeScraper(HttpClient client) {
            Client = client;
        }

        public List<string> Scrape(string word) {
            /*var html = Client.GetStringAsync("http://www.prime-rhyme.com/" + word + ".html").GetResult();
            var document = CQ.CreateDocument(html);
            var blocks = document.Select(".list_block");
            var cells = blocks.Select("td");
            var results = cells.Elements.Select(each => each.InnerHTML).ToList();

            return results;*/

            return new List<string>();
        }

        public HttpClient Client { get; set; }
    }
}