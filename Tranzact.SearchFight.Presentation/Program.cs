using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using Newtonsoft.Json;
using System.Text;
using Tranzact.SearchFight.Presentation.Entities.INPUT;
using Tranzact.SearchFight.Transversal;
using Tranzact.SearchFight.Presentation.Entities.OUTPUT;
using System.Threading.Tasks;

namespace Tranzact.SearchFight.Presentation
{
    class Program
    {
        public static IConfiguration _configuration;
        static async System.Threading.Tasks.Task Main(string[] args)
        {
            SetUpConfiguration();

            Console.WriteLine();
            string query = Console.ReadLine().ToString();

            var listTotals = await GetTotals(query);
            PrintSearch(listTotals);
            PrintWinners(listTotals);
            PrintTotalWinner(listTotals);
        }
        private static void SetUpConfiguration()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
        }
        private static async Task<List<SearchOUT>> GetTotals(String query)
        {
            List<SearchOUT> Searchs = new List<SearchOUT>();
            string urlSearchService = _configuration["urlSearchService"];

            var engines = _configuration.GetSection("engines")
                                        .GetChildren()
                                        .Select(x => x.Value)
                                        .ToArray(); ;

            foreach (var engine in engines)
            {
                using (var httpClient = new HttpClient())
                {
                    var payload = JsonConvert.SerializeObject(new SearchIN() { engine = engine, query = query });

                    var request = new HttpRequestMessage
                    {
                        RequestUri = new Uri($"{urlSearchService}/SearchEngine/GetSearchTotals"),
                        Method = new HttpMethod("Get"),
                        Content = new StringContent(payload, Encoding.UTF8, "application/json")
                    };

                    var response = await httpClient.SendAsync(request);
                    var result = await response.Content.ReadAsStringAsync();
                    var searchOUT = JsonConvert.DeserializeObject<Response<SearchOUT>>(result);
                    request.Dispose();

                    if (searchOUT.Status)
                        Searchs.AddRange(searchOUT.List);
                }
            }
            return Searchs;
        }
        private static void PrintSearch(List<SearchOUT> lista)
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
        private static void PrintWinners(List<SearchOUT> lista)
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
        private static void PrintTotalWinner(List<SearchOUT> lista)
        {
            var winnerWord = lista.OrderByDescending(e => e.totalResults).FirstOrDefault().word;
            var printSearch = $"Total winner : {winnerWord}";
            Console.WriteLine(printSearch);
        }
    }
}