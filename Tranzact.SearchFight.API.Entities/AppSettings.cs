using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.API.Entities
{
    public class AppSettings
    {
        public GoogleEngine googleEngine { get; set; }
        public MSNEngine msnEngine { get; set; }

    }

    public class GoogleEngine
    {
        public string apiKey { get; set; }
        public string cx { get; set; }
        public string baseUrl { get; set; }
    }
    public class MSNEngine
    {
        public string apiKey { get; set; }
        public string baseUrl { get; set; }
    }
}
