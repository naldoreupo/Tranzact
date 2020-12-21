using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.Transversal
{
    public class Response<TEntity>
    {
        public bool Status { get; set; }
        public string Message { get; set; }

        public TEntity Data { get; set; }

        public List<TEntity> List { get; set; }
    }
}
