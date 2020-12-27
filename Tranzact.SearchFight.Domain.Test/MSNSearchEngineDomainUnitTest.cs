using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using Tranzact.SearchFight.API.Entities;
using Tranzact.SearchFight.Domain.SearchEngine;
using Tranzact.SearchFight.Transversal;
using Xunit;

namespace Tranzact.SearchFight.Domain.Test
{
    public class MSNSearchEngineDomainUnitTest
    {
        private readonly IOptions<GoogleEngine> _appSettings = Substitute.For<IOptions<GoogleEngine>>();
        private readonly IMapper _mapper;

        private readonly IConfigurationRoot _config;

        public MSNSearchEngineDomainUnitTest()
        {
            _config = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json")
                .Build();

            _mapper = Maps.InitMapper();
        }

        [Theory]
        [InlineData(".net java \"java script\"")]
        [InlineData(".net java ")]
        public async void GetTotals_InputValid_ReturnOk(string query)
        {
            // Arrange           
            var optionValue = _config.GetSection("GoogleEngine").Get<MSNEngine>();
            var options = Options.Create<MSNEngine>(optionValue);

            var response = "{\"webPages\":{\"totalEstimatedMatches\":107000000 }}";
            var messageHandler = new MockHttpMessageHandler(response, HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://not-important.com")
            };
            var googleSearchEngineDomain = new MSNSearchEngineDomain(_mapper, options, httpClient);

            //Act
            var words = query.SplitBySpace();
            var result = await googleSearchEngineDomain.GetSearchTotals(words);

            // Assert
            Assert.True(result.Status);
            Assert.Equal(words.Count(), result.List.Count());
        }
    }
}
