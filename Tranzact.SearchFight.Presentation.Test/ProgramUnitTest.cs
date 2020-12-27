using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Mvc;
using Tranzact.SearchFight.Domain.Test;
using Tranzact.SearchFight.Transversal;
using Xunit;

namespace Tranzact.SearchFight.Presentation.Test
{
    public class ProgramUnitTest
    {
        [Theory]
        [InlineData("Google,MSN", ".net java ")]
        public async System.Threading.Tasks.Task GetTotals_ValidInput_ReturnOkAsync(string engines, string query)
        {
            // Arrange   
            var listEngines = engines.Split(",").ToList();


            //var result ="";
                                    
            var response = @"{
                                'status': true,
                                'message': null,
                                'data': null,
                                'list': [
                                    {
                                        'engine': 'Google',
                                        'word': '.net',
                                        'totalResults': 84300000
                                    },
                                    {
                                        'engine': 'Google',
                                        'word': 'Java',
                                        'totalResults': 56300000
                                    }
                                ]
                            }";

            var messageHandler = new MockHttpMessageHandler(response, HttpStatusCode.InternalServerError);
            var httpClient = new HttpClient(messageHandler)
            {
                BaseAddress = new Uri("http://not-important.com")
            };
            var searchEngine = new SearchEngine(httpClient);

            // Act
            var result = await searchEngine.GetTotals(listEngines, query);


            // Assert
            Assert.NotNull(result);

        }
    }
}
