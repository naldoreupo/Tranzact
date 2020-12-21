using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
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

        private readonly IMapper _mapper;
        private readonly InterfaceFactorySearchEngine _searchEngine;
        public SearchEngineController(IMapper mapper, InterfaceFactorySearchEngine searchEngine)
        {
            _mapper = mapper;
            _searchEngine = searchEngine;
        }

        [HttpGet]
        public async Task<IActionResult> SearchText([FromBody]  SearchIN searchIN)
        {
            try
            {
                if (searchIN.engine != EngineConstants.Google && searchIN.engine != EngineConstants.MSN)
                    return BadRequest($"{searchIN.engine} is not currently supported");

                var _engineDomain = _searchEngine.Build(searchIN.engine);
                var result = await _engineDomain.Search(searchIN.query);

                return Ok(result);
            }
            catch (Exception e)
            {
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }
        }
    }
}
