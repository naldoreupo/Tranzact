
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tranzact.SearchFight.Presentation.Entities.INPUT;
using Tranzact.SearchFight.Presentation.Entities.OUTPUT;
using Tranzact.SearchFight.Transversal;

namespace Tranzact.SearchFight.Presentation
{
    public class SearchEngine
    {
        public HttpClient _httpClient;
        public SearchEngine(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        public async Task<List<SearchOUT>> GetTotals(List<string> engines, String query)
        {
            List<SearchOUT> Searchs = new List<SearchOUT>();


            foreach (var engine in engines)
            {
                var payload = Newtonsoft.Json.JsonConvert.SerializeObject(new SearchIN() { engine = engine, query = query });

                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri($"{_httpClient.BaseAddress}SearchEngine/GetSearchTotals"),
                    Method = new HttpMethod("Get"),
                    Content = new StringContent(payload, Encoding.UTF8, "application/json")
                };

                var responseBody = await _httpClient.SendAsync(request);
                var result = await responseBody.Content.ReadAsStringAsync();
                var searchOUT = Newtonsoft.Json.JsonConvert.DeserializeObject<Response<SearchOUT>>(result);
                request.Dispose();

                if (searchOUT.Status)
                    Searchs.AddRange(searchOUT.List);

            }
            return Searchs;
        }
        public void PrintSearch(List<SearchOUT> lista)
        {
            var words = lista.Select(e => e.word).Distinct();
            var engines = lista.Select(e => e.engine).Distinct();

            foreach (var word in words)
            {
                string printSearch = $"{word}";
                foreach (var engine in engines)
                {
                    var total = lista.Where(e => e.engine == engine && e.word == word).Single().totalResults;
                    printSearch += $" {engine} : {total}";
                }
                Console.WriteLine(printSearch);
            }
        }
        public void PrintWinners(List<SearchOUT> lista)
        {
            var words = lista.Select(e => e.word).Distinct();
            var engines = lista.Select(e => e.engine).Distinct();

            foreach (var engine in engines)
            {
                var winnerWord = lista.Where(e => e.engine == engine).OrderByDescending(e => e.totalResults).FirstOrDefault().word;
                var printSearch = $"{engine}  winner : {winnerWord}";
                Console.WriteLine(printSearch);
            }
        }
        public void PrintTotalWinner(List<SearchOUT> lista)
        {
            var winnerWord = lista.OrderByDescending(e => e.totalResults).FirstOrDefault().word;
            var printSearch = $"Total winner : {winnerWord}";
            Console.WriteLine(printSearch);
        }
    }
}
