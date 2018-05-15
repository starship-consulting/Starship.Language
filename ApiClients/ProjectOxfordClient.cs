using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Starship.Language.ApiClients {
    public class ProjectOxfordClient {
        public ProjectOxfordClient() : this(new HttpClient()) {
        }

        public ProjectOxfordClient(HttpClient client) {
            Client = client;
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", "");
        }
        
        public Dictionary<string, double> GetProbability(params string[] texts) {

            if (texts.Count() == 0) {
                return new Dictionary<string, double>();
            }

            if (texts.Count() > 1000) {
                throw new Exception("Max entries of 1000 exceeded.");
            }

            var data = new {
                queries = texts.ToList()
            };

            var results = new Dictionary<string, double>();

            var response = Client.PostAsJsonAsync("https://api.projectoxford.ai/text/weblm/v1.0/calculateJointProbability?model=body", data).Result;
            var result = response.Content.ReadAsStringAsync().Result;
            var json = JsonConvert.DeserializeObject(result) as JObject;
            
            foreach (var item in json["results"].ToArray()) {
                results.Add(item.Value<string>("words"), item["probability"].Value<double>());
            }
            
            return results;
        }

        public HttpClient Client { get; set; }
    }
}