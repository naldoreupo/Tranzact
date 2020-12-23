using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.Domain.Entities
{
    public class MSNResponse
    {
        public WebPages webPages { get; set; }
    }

    public class WebPages
    {
      public int totalEstimatedMatches { get; set; }
    }
}
