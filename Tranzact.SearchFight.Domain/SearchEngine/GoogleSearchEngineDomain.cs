using Google.Apis.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Tranzact.SearchFight.API.Entities;
using Tranzact.SearchFight.API.Entities.OUTPUT;
using Tranzact.SearchFight.Domain.Entities;
using Tranzact.SearchFight.Domain.Interface;
using Tranzact.SearchFight.Transversal;
using System.Linq;
using System.Collections;

namespace Tranzact.SearchFight.Domain.SearchEngine
{
    public class GoogleSearchEngineDomain : InterfaceSearchEngineDomain
    {
        public string Engine => EngineConstants.Google;
        private readonly AppSettings _appSettings;
        public GoogleSearchEngineDomain(AppSettings appSettings)
        {
            _appSettings = appSettings;
        }

        public async Task<Response<SearchOUT>> GetTotals(List<string> words)
        {
            try
            {
                var listTotals = new List<SearchOUT>();

                foreach (var word in words)
                {
                    listTotals.Add(await SearchEngine(word));
                }


                return new Response<SearchOUT>() { Status = true, List = listTotals };
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

        private async Task<SearchOUT> SearchEngine(string word)
        {
            string apiKey = _appSettings.googleEngine.apiKey;
            string cx = _appSettings.googleEngine.cx;
            var customsearchUrl = $"{_appSettings.googleEngine.baseUrl}/customsearch/v1?cx={cx}&key={apiKey}&q={word}";
            var googleResponse = new GoogleResponse();
            var searchOUT = new SearchOUT();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(customsearchUrl);
                var result = await response.Content.ReadAsStringAsync();
                googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(result);
                searchOUT = new SearchOUT()
                {
                    word = word,
                    totalRecords = googleResponse.SearchInformation.totalResults,
                    engine = this.Engine
                };
            }
            return searchOUT;
        }
    }
}
