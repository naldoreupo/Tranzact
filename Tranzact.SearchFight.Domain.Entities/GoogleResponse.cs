using Newtonsoft.Json;
using System;

namespace Tranzact.SearchFight.Domain.Entities
{
    public class GoogleResponse
    {
        [JsonProperty("searchInformation")]
        public SearchInformation SearchInformation { get; set; }
    }

   public class SearchInformation
    {

        [JsonProperty("totalResults")]
        public long totalResults { get; set; }
    }
}
