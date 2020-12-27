using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Tranzact.SearchFight.Presentation
{
    class Program
    {
        private static IConfiguration _configuration;
        private static SearchEngine _searchEngine;

        static async Task Main(string[] args)
        {
            SetUp();

            var engines = _configuration.GetSection("engines")
                                        .GetChildren()
                                        .Select(x => x.Value)
                                        .ToList();
            Console.WriteLine();
            string query = Console.ReadLine().ToString();

            var listTotals = await _searchEngine.GetTotals(engines, query);
            _searchEngine.PrintSearch(listTotals);
            _searchEngine.PrintWinners(listTotals);
            _searchEngine.PrintTotalWinner(listTotals);
        }

        private static void SetUp()
        {
            _configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            var services = new ServiceCollection();
            services.AddTransient<SearchEngine>();
            services.AddHttpClient<SearchEngine>(c => c.BaseAddress = new Uri(_configuration["urlSearchService"]));
            var serviceProvider = services.BuildServiceProvider();
            _searchEngine = serviceProvider.GetService<SearchEngine>();
        }
    }
}