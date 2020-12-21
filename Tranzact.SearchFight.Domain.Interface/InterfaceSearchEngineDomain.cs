using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranzact.SearchFight.API.Entities.OUTPUT;
using Tranzact.SearchFight.Transversal;

namespace Tranzact.SearchFight.Domain.Interface
{
    public interface InterfaceSearchEngineDomain
    {
        string Engine { get; }
        Task<Response<SearchOUT>> Search(string query);
    }
}
