using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Models.Repositories;
using RestAPI.Models.Search;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {

        private IServiceRepository repository;

        public RouteController(IServiceRepository repo)
        {
            repository = repo;
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("products")]
        public ActionResult ShortestRouteForProducts([FromQuery] OptimalRoute engine)
        {
            if(engine.ProductNames.Count > 5)
            {
                return BadRequest("Product list limit exceeded.");
            }
            List<Business> best = engine.FindBusinesses(repository);
            foreach(var b in best)
            {
                b.EliminateDepth();
            }
            if(!best.Any())
            {
                return NoContent();
            }
            return Ok(best);
        }

    }
}
