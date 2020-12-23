using AutoMapper;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using Tranzact.SearchFight.API.Entities;
using Tranzact.SearchFight.Domain.SearchEngine;
using Xunit;

namespace Tranzact.SearchFight.Domain.Test
{
    public class GoogleSearchEngineDomainUnitTest
    {
        private readonly IMapper _mapper = Substitute.For<IMapper>();
        private readonly IOptions<GoogleEngine> _appSettings = Substitute.For<IOptions<GoogleEngine>>();
        [Fact]
        public void GetTotals_InputValid_ReturnOk()
        {
            // Arrange
            var googleSearchEngineDomain = new GoogleSearchEngineDomain(_mapper, _appSettings);

            // Act
            var result = googleSearchEngineDomain.GetSearchTotals(new List<string>() { });

            // Assert
        }
    }
}
