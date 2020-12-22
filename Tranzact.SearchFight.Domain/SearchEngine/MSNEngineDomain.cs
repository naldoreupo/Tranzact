using System;
using System.Collections.Generic;
//using Microsoft.Azure.CognitiveServices.Search.WebSearch;
//using Microsoft.Azure.CognitiveServices.Search.WebSearch.Models;
using System.Linq;
using System.Threading.Tasks;
using Tranzact.SearchFight.API.Entities.OUTPUT;
using Tranzact.SearchFight.Domain.Interface;
using Tranzact.SearchFight.API.Entities;
using Tranzact.SearchFight.Transversal;
using Tranzact.SearchFight.Domain.Entities;
using Newtonsoft.Json;
using Microsoft.Azure.CognitiveServices.Search.WebSearch;

namespace Tranzact.SearchFight.Domain.SearchEngine
{
    public class MSNEngineDomain : InterfaceSearchEngineDomain
    {
        public string Engine => EngineConstants.MSN;
        private readonly AppSettings _appSettings;
        public MSNEngineDomain(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<Response<SearchOUT>> GetTotals(List<string> words)
        {
            try
            {
                Random _random = new Random();
                var list = new List<SearchOUT>();

                foreach (var word in words)
                {
                    list.Add(new SearchOUT() { word = word, totalRecords = _random.Next(500, 600), engine = this.Engine });
                }

                return new Response<SearchOUT>()
                {
                    Status = true,
                    List = list
                };
            }
            catch (Exception ex)
            {
                return new Response<SearchOUT>()
                {
                    Status = false,
                    Message = "Error getting data"
                };
            }
        }

        public async Task<SearchOUT> SearchEngine(string word)
        {
            var client = new WebSearchClient(new ApiKeyServiceClientCredentials(_appSettings.msnEngine.apiKey));
            var webData = await client.Web.SearchAsync(query: word);

            return new SearchOUT()
            {
                word = word,
                totalRecords = webData.WebPages.Value.Count,
                engine = this.Engine
            };
        }
    }
}
