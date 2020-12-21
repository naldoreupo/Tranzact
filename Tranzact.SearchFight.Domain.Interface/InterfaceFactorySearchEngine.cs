using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.Domain.Interface
{
   public interface InterfaceFactorySearchEngine
    {
        InterfaceSearchEngineDomain Build(string engine);
    }
}
