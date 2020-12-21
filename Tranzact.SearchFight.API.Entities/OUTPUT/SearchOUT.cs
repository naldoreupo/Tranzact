using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.API.Entities.OUTPUT
{
    public class SearchOUT
    {
        public string engine { get; set; }
        public string word { get; set; }
        public int totalRecords { get; set; }
    }
}
