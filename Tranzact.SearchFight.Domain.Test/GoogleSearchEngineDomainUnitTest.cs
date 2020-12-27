using AutoMapper;
using Microsoft.Extensions.Options;
using NSubstitute;
using System;
using System.Collections.Generic;
using System.Net.Http;
using Tranzact.SearchFight.API.Entities;
using Tranzact.SearchFight.Domain.SearchEngine;
using Xunit;
using Tranzact.SearchFight.Transversal;
using System.Net;
using Moq;
using System.Threading.Tasks;
using System.Threading;
using Microsoft.Extensions.Configuration;
using System.Linq;

namespace Tranzact.SearchFight.Domain.Test
{
    public class GoogleSearchEngineDomainUnitTest
    {
        private readonly IOptions<GoogleEngine> _appSettings = Substitute.For<IOptions<GoogleEngine>>();
        private readonly IMapper _mapper;

        private readonly IConfigurationRoot _config;

        public GoogleSearchEngineDomainUnitTest()
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
            var optionValue = _config.GetSection("GoogleEngine").Get<GoogleEngine>();
            var options = Options.Create<GoogleEngine>(optionValue);

            var response = "{\"searchInformation\": { \"totalResults\": \"10570000000\"}}";
            var messageHandler = new MockHttpMessageHandler(response, HttpStatusCode.OK);
            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://not-important.com")
            };
            var googleSearchEngineDomain = new GoogleSearchEngineDomain(_mapper, options, httpClient);

            //Act
            var words = query.SplitBySpace();
            var result = await googleSearchEngineDomain.GetSearchTotals(query.SplitBySpace());

            // Assert
            Assert.True(result.Status);
            Assert.Equal(words.Count(), result.List.Count());
        }
    }
}
