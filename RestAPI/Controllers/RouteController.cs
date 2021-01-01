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
            if (engine.ProductNames.Count > 3)
            {
                return BadRequest("Product list limit exceeded.");
            }
            List<Business> best = engine.FindBusinesses(repository);
            foreach (var b in best)
            {
                b.EliminateDepth();
                b.PictureData = null;
            }
            if (!best.Any())
            {
                return NoContent();
            }
            return Ok(best);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("courier")]
        public ActionResult ShortestRouteForCourier([FromQuery] OptimalCourierRoute engine)
        {
            if(engine.OrdersCount > 2)
            {
                return BadRequest("Orders limit exceeded.");
            }
            //List<Business> best = engine.GetRoute(repository);
            return null;
        }

    }
}
