using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Tranzact.SearchFight.Transversal
{
 
    public static class MyExtensions
    {
        public static List<string> SplitBySpace(this String str)
        {
            return  new Regex("((?<=\")[^\"]*(?=\"( |$)+)|(?<= |^)[^ \"]*(?= |$))").Matches(str)
                             .Select(m => m.Value)
                             .ToList();
        }
    }
}
