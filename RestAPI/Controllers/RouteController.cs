using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Models.Search;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RouteController : ControllerBase
    {

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("products/{id}")]
        public ActionResult ShortestRouteForProducts([FromQuery] OptimalRoute engine)
        {
            if(engine.ProductNames.Count > 5)
            {
                return BadRequest("Product list limit exceeded.");
            }
            List<Business> best = engine.FindBusinesses();
            if(!best.Any())
            {
                return NoContent();
            }
            return Ok(best);
        }

    }
}
