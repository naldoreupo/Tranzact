using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranzact.SearchFight.API.Entities.OUTPUT;
using Tranzact.SearchFight.Domain.Entities;
using Tranzact.SearchFight.Transversal;

namespace Tranzact.SearchFight.Domain.Interface
{
    public interface InterfaceSearchEngineDomain
    {
        string Engine { get; }
        Task<Response<SearchOUT>> GetSearchTotals(List<string> words);
        Task<ApiResponse> InvokeSearchEngineAPI(string word);
    }
}
