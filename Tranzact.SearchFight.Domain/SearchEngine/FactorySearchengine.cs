using System;
using System.Collections.Generic;
using System.Text;
using Tranzact.SearchFight.Domain.Interface;
using System.Linq;

namespace Tranzact.SearchFight.Domain.SearchEngine
{
    public class FactorySearchengine : InterfaceFactorySearchEngine
    {
        private readonly IEnumerable<InterfaceSearchEngineDomain> _searchEngineDomains;
        public FactorySearchengine(IEnumerable<InterfaceSearchEngineDomain> searchEngineDomains)
        {
            _searchEngineDomains = searchEngineDomains;
        }
        public InterfaceSearchEngineDomain Build(string engineCode)
        {
            try
            {
                return _searchEngineDomains.SingleOrDefault(engine => engine.Engine == engineCode);
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
