using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Tranzact.SearchFight.API.Entities.INPUT;
using Tranzact.SearchFight.Domain.Interface;
using Tranzact.SearchFight.Transversal;

namespace Tranzact.SearchFight.Service.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SearchEngineController : ControllerBase
    {
        private readonly InterfaceFactorySearchEngine _searchEngine;
        public SearchEngineController(InterfaceFactorySearchEngine searchEngine)
        {
            _searchEngine = searchEngine;
        }

        [HttpGet]
        [Route("GetSearchTotals")]
        public async Task<IActionResult> GetSearchTotals([FromBody] SearchIN searchIN)
        {
            try
            {
                if (searchIN.engine != EngineConstants.Google && searchIN.engine != EngineConstants.MSN)
                    return BadRequest($"{searchIN.engine} is not currently supported");

                var _engineDomain = _searchEngine.Build(searchIN.engine);
                var result = await _engineDomain.GetSearchTotals(searchIN.query.SplitBySpace());

                return Ok(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
