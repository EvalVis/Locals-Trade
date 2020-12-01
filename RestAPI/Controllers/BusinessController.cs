using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using RestAPI.Models.BindingTargets;
using RestAPI.Models.Repositories;

namespace RestAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private IServiceRepository repository;

        public BusinessController(IServiceRepository repo)
        {
            repository = repo;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("All")]
        public ActionResult<IEnumerable<Business>> GetBusinesses()
        {
            IEnumerable<Business> businesses = repository.Business.
                Include(b => b.User).
                Include(b => b.Products).
                Include(b => b.Workdays);
            if (!businesses.Any()) return NoContent();
            foreach (var b in businesses)
            {
                b.EliminateDepth();
            }
            return Ok(businesses);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("Filtered")]
        public ActionResult<IEnumerable<Business>> GetFilteredBusinesses(SearchEngine searchEngine)
        {
            IEnumerable<Business> filteredBusinesses = searchEngine.FilterBusinesses(repository);
            foreach (var b in filteredBusinesses)
            {
                b.EliminateDepth();
            }
            if (!filteredBusinesses.Any()) return NoContent();
;            return Ok(filteredBusinesses);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> Business(long id)
        {
            Business business = await repository.Business.
                Include(b => b.Workdays).Include(b => b.Products).Include(b => b.User).
                FirstOrDefaultAsync(b => b.BusinessID == id);
            if (business == null) return NotFound();
            business.EliminateDepth();
            return Ok(business);
        }

        [HttpDelete("{id}")]
        public async Task RemoveBusiness(long id)
        {
            await repository.RemoveBusinessAsync(new Business {BusinessID = id});
        }

        [HttpPost]
        public async Task<ActionResult> SaveBusiness(BusinessBindingTarget target)
        {
            await repository.SaveBusinessAsync(target.ToBusiness());
            return Ok();
        }

        [HttpPut]
        public async Task<ActionResult> UpdateBusiness(Business business)
        {
            await repository.UpdateBusinessAsync(business);
            return Ok();
        }

    }
}
