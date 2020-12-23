using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NSubstitute;
using System.Collections.Generic;
using Tranzact.SearchFight.API.Entities.INPUT;
using Tranzact.SearchFight.API.Entities.OUTPUT;
using Tranzact.SearchFight.Domain.Interface;
using Tranzact.SearchFight.Domain.SearchEngine;
using Tranzact.SearchFight.Service.Controllers;
using Tranzact.SearchFight.Transversal;
using Xunit;

namespace Tranzact.SearchFight.Service.Test
{
    public class SearchEngineControllerTest
    {
        private readonly InterfaceFactorySearchEngine _mockFactorySearchEngine = Substitute.For<InterfaceFactorySearchEngine>();
        private readonly InterfaceSearchEngineDomain _mockSearchEngineDomain = Substitute.For<InterfaceSearchEngineDomain>();

        [Theory]
        [InlineData("Yandex")]
        [InlineData("Gibiru")]
        public async System.Threading.Tasks.Task SearchText_InputInvalidEngine_ReturnException(string engine)
        {
            // Arrange
            var searchIN = new SearchIN() { engine = engine };
            var searchEngineController = new SearchEngineController(_mockFactorySearchEngine);

            // Act
            var actionResult = await searchEngineController.GetSearchTotals(searchIN);
            var requestResult = actionResult as ObjectResult;

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, requestResult.StatusCode);
        }

        [Theory]
        [InlineData(".net java \"java script\"")]
        [InlineData(".net java ")]
        public async System.Threading.Tasks.Task SearchText_InputValid_ReturnOk(string query)
        {
            // Arrange
            var searchIN = new SearchIN() { engine = EngineConstants.Google, query = query };
            var listTotals = new List<SearchOUT>();
            var words = query.SplitBySpace();
            var respon = new Response<SearchOUT>() { Status = true, List = listTotals };

            foreach (var word in words)
            {
                listTotals.Add(new SearchOUT() { engine = EngineConstants.Google, word = word });
            }

            _mockSearchEngineDomain.GetSearchTotals(words).ReturnsForAnyArgs(respon);
            _mockFactorySearchEngine.Build(EngineConstants.Google).ReturnsForAnyArgs(_mockSearchEngineDomain);
            var searchEngineController = new SearchEngineController(_mockFactorySearchEngine);

            // Act
            var actionResult = await searchEngineController.GetSearchTotals(searchIN);
            var requestResult = actionResult as ObjectResult;
            var totals = (Response<SearchOUT>)requestResult.Value;

            // Assert
            Assert.Equal(StatusCodes.Status200OK, requestResult.StatusCode);
            Assert.Equal(words.Count, totals.List.Count);
        }
    }
}

