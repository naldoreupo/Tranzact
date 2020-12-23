using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Tranzact.SearchFight.Domain.Entities;

namespace Tranzact.SearchFight.Transversal
{
    public static class Maps
    {
        public static IMapper InitMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<GoogleResponse,ApiResponse>()
                        .ForMember(x => x.totalResults, opt => opt.MapFrom(model => model.SearchInformation.totalResults)); 

                cfg.CreateMap<MSNResponse, ApiResponse>()
                        .ForMember(x => x.totalResults, opt => opt.MapFrom(model => model.webPages.totalEstimatedMatches)); 
            });

            return config.CreateMapper();
        }
    }
}
