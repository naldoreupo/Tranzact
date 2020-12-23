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
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Tranzact.SearchFight.Domain.SearchEngine
{
    public class GoogleSearchEngineDomain : InterfaceSearchEngineDomain
    {
        public string Engine => EngineConstants.Google;
        private readonly IMapper _mapper;
        private readonly IOptions<GoogleEngine> _config;
        public GoogleSearchEngineDomain( IMapper mapper, IOptions<GoogleEngine> config)
        {
            _mapper = mapper;
            _config = config;
        }

        public async Task<Response<SearchOUT>> GetSearchTotals(List<string> words)
        {
            try
            {
                var listTotals = new List<SearchOUT>();

                foreach (var word in words)
                {
                    var apiResponse = await InvokeSearchEngineAPI(word);
                    listTotals.Add(new SearchOUT()
                    {
                        word = word,
                        totalResults = apiResponse.totalResults,
                        engine = this.Engine
                    });
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

        public async Task<ApiResponse> InvokeSearchEngineAPI(string word)
        {
            string apiKey = _config.Value.apiKey;
            string cx = _config.Value.cx;
            var customsearchUrl = $"{_config.Value.baseUrl}/customsearch/v1?cx={cx}&key={apiKey}&q={word}";
            var googleResponse = new GoogleResponse();

            using (var client = new HttpClient())
            {
                var response = await client.GetAsync(customsearchUrl);
                var result = await response.Content.ReadAsStringAsync();
                googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(result);
            }

            return _mapper.Map<GoogleResponse, ApiResponse>(googleResponse);
        }
    }
}
