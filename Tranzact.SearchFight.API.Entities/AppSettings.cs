using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.API.Entities
{
    public class GoogleEngine : InterfaceGoogleEngine
    {
        public string apiKey { get; set; }
        public string cx { get; set; }
        public string baseUrl { get; set; }
    }
    public class MSNEngine : InterfaceMSNEngine
    {
        public string apiKey { get; set; }
        public string baseUrl { get; set; }
    }
}
