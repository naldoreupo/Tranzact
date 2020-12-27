using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tranzact.SearchFight.API.Entities.OUTPUT;
using Tranzact.SearchFight.Domain.Interface;
using Tranzact.SearchFight.API.Entities;
using Tranzact.SearchFight.Transversal;
using Tranzact.SearchFight.Domain.Entities;
using Newtonsoft.Json;
using System.Net.Http;
using AutoMapper;
using Microsoft.Extensions.Options;

namespace Tranzact.SearchFight.Domain.SearchEngine
{
    public class MSNEngineDomain : InterfaceSearchEngineDomain
    {
        public string Engine => EngineConstants.MSN;
        public HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IOptions<MSNEngine> _config;
        public MSNEngineDomain(IMapper mapper, IOptions<MSNEngine> config, HttpClient httpClient)
        {
            _config = config;
            _mapper = mapper;
            _httpClient = httpClient;
        }

        public async Task<Response<SearchOUT>> GetSearchTotals(List<string> words)
        {
            try
            {
                var list = new List<SearchOUT>();

                foreach (var word in words)
                {
                    var apiResponse = await InvokeSearchEngineAPI(word);
                    list.Add(new SearchOUT()
                    {
                        word = word,
                        totalResults = apiResponse.totalResults,
                        engine = this.Engine
                    });
                }
                return new Response<SearchOUT>() { Status = true, List = list };
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
            var customsearchUrl = $"{_config.Value.baseUrl}/search?q={word}";
            var msnResponse = new MSNResponse();

            _httpClient.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", apiKey);
            var response = await _httpClient.GetAsync(customsearchUrl);
            var result = await response.Content.ReadAsStringAsync();
            msnResponse = JsonConvert.DeserializeObject<MSNResponse>(result);

            return _mapper.Map<MSNResponse, ApiResponse>(msnResponse);
        }
    }
}