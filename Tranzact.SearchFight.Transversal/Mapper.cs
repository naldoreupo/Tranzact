using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;

namespace Tranzact.SearchFight.Transversal
{
    public static class Maps
    {
        public static IMapper InitMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                //cfg.CreateMap<PurchaseInput, PurchaseDTO>();


            });

            return config.CreateMapper();
        }
    }
}
