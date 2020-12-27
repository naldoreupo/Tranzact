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
using Google.Apis.Http;

namespace Tranzact.SearchFight.Domain.SearchEngine
{
    public class GoogleSearchEngineDomain : InterfaceSearchEngineDomain
    {
        public string Engine => EngineConstants.Google;
        public HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IOptions<GoogleEngine> _config;
        public GoogleSearchEngineDomain(IMapper mapper, IOptions<GoogleEngine> config, HttpClient httpClient)
        {
            _mapper = mapper;
            _config = config;
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri(_config.Value.baseUrl);
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
            var customsearchUrl = $"/customsearch/v1?cx={cx}&key={apiKey}&q={word}";

            var response = await _httpClient.GetAsync(customsearchUrl);
            var result = await response.Content.ReadAsStringAsync();
            var googleResponse = JsonConvert.DeserializeObject<GoogleResponse>(result);

            return _mapper.Map<GoogleResponse, ApiResponse>(googleResponse);
        }
    }
}
