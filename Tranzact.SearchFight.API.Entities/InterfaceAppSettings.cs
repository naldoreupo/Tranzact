using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.API.Entities
{
    public interface InterfaceGoogleEngine
    {
        public string apiKey { get; set; }
        public string cx { get; set; }
        public string baseUrl { get; set; }
    }
    public interface InterfaceMSNEngine
    {
        public string apiKey { get; set; }
        public string baseUrl { get; set; }
    }
}
