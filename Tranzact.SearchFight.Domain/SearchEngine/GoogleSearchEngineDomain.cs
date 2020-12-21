using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Tranzact.SearchFight.API.Entities.OUTPUT;
using Tranzact.SearchFight.Domain.Interface;
using Tranzact.SearchFight.Transversal;

namespace Tranzact.SearchFight.Domain.SearchEngine
{
    public class GoogleSearchEngineDomain : InterfaceSearchEngineDomain
    {
        public string Engine => EngineConstants.Google;

        public async Task<Response<SearchOUT>> Search(string query)
        {
            try
            {
                Random _random = new Random();
                string[] words = query.Split(" ");
                var list = new List<SearchOUT>();

                foreach (var word in words)
                {
                    list.Add(new SearchOUT() { word = word, totalRecords = _random.Next(500,600), engine = this.Engine });
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
    }
}
