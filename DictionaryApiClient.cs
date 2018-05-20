using System.Net.Http;

namespace Starship.Language {
    public class DictionaryApiClient {

        public DictionaryApiClient() {
            Client = new HttpClient();
        }

        public DictionaryApiClient(HttpClient client) {
            Client = client;
        }

        public Word GetPronunciation(string input) {

            /*input = input.ToLower();
            var html = Client.GetStringAsync("http://www.dictionary.com/browse/" + input).Result;
            var document = CQ.CreateDocument(html);
            var syllables = document.Select("[data-syllable].me").Attr("data-syllable");
            var pronunciation = document.Select(".pron.spellpron").First().Text();

            if (pronunciation.Contains(";")) {
                var segments = pronunciation.Split(';');
                pronunciation = segments.First().Replace("stressed", "");
            }

            if (pronunciation.Contains(",")) {
                pronunciation = pronunciation.Split(',').First();
            }

            pronunciation = pronunciation.Replace(" ", "");

            return new Word {
                Text = input
            };*/

            return new Word();

            //return new PronouncedWord {
            //    Word = input,
                //Syllables = syllables.Split('·').ToList(),
                //Pronunciation = pronunciation.Replace("[", "").Replace("]", "").Split('-').ToList()
            //};
        }

        public HttpClient Client { get; set; }
    }
}