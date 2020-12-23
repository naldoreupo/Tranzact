using Newtonsoft.Json;
using System;

namespace Tranzact.SearchFight.Domain.Entities
{
    public class GoogleResponse
    {
        [JsonProperty("SearchInformation")]
        public SearchInformation SearchInformation { get; set; }
    }

   public class SearchInformation
    {

        [JsonProperty("TotalResults")]
        public long totalResults { get; set; }
    }
}
