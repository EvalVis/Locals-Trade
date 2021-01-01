using System.Collections.Generic;
using System.Diagnostics;
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
            Stopwatch watch = new Stopwatch();
            if (engine.ProductNames.Count > 4)
            {
                return BadRequest("Product list limit exceeded.");
            }
            watch.Start();
            List<Business> best = engine.FindBusinesses(repository);
            watch.Stop();
            System.Diagnostics.Debug.WriteLine(watch.Elapsed);
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
            if(engine.OrdersCount > 3)
            {
                return BadRequest("Orders limit exceeded.");
            }
            //TODO: find best routes.
            return null;
        }

    }
}
