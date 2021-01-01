using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using RestAPI.Cryptography;
using RestAPI.Models;
using RestAPI.Models.Search;
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
        private long claimedId;
        private int pageSize = 10;

        public BusinessController(IServiceRepository repo, IHttpContextAccessor accessor)
        {
            claimedId = long.Parse(accessor.HttpContext.User.Claims.FirstOrDefault(type => type.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            repository = repo;
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("All/{page}")]
        public ActionResult<PageBusiness> GetBusinesses(int page = 1)
        {
            long totalItems = repository.Business.Count();
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            if (page < 1) return BadRequest();
            if (page > totalPages) return NotFound();
            IEnumerable<Business> businesses = repository.Business.
                Include(b => b.User).
                Include(b => b.Products).
                Include(b => b.Workdays).OrderByDescending(b => b.BusinessID).Skip((page - 1) * pageSize).Take(pageSize);
            if (!businesses.Any()) return NoContent();
            foreach (var b in businesses)
            {
                b.EliminateDepth();
                b.PictureData = null;
            }
            PageBusiness pageBusiness = new PageBusiness { TotalPages = totalPages, Businesses = businesses };
            return Ok(pageBusiness);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [HttpGet("Filtered/{page}")]
        public ActionResult<PageBusiness> GetFilteredBusinesses([FromQuery] SearchEngine searchEngine, int page = 1)
        {
            if (page < 1 || searchEngine.WeekdaySelected.Length != 7) return BadRequest();
            IEnumerable<Business> filteredBusinesses = searchEngine.FilterBusinesses(page, pageSize, repository, out int totalItems);
            int totalPages = (int)Math.Ceiling((decimal)totalItems / pageSize);
            if (page > totalPages) return NotFound();
            foreach (var b in filteredBusinesses)
            {
                b.EliminateDepth();
                b.PictureData = null;
            }
            if (totalItems == 0) return NoContent();
            PageBusiness filteredPageBusiness = new PageBusiness { TotalPages = totalPages, Businesses = filteredBusinesses };
            return Ok(filteredPageBusiness);
        }

        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet("User")]
        public ActionResult<IEnumerable<Business>> GetUserBusinesses()
        {
            IEnumerable<Business> businesses = repository.Business.
                Include(b => b.User).
                Include(b => b.Products).
                Include(b => b.Workdays).Where(b => b.UserID == claimedId);
            if (!businesses.Any()) return NoContent();
            foreach (var b in businesses)
            {
                b.EliminateDepth();
                b.PictureData = null;
            }
            return Ok(businesses);
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet("{id}")]
        public async Task<ActionResult<Business>> Business(long id)
        {
            if (id < 1) return BadRequest();
            Business business = await repository.Business.
                Include(b => b.Workdays).Include(b => b.Products).Include(b => b.User).
                FirstOrDefaultAsync(b => b.BusinessID == id);
            if (business == null) return NotFound();
            business.EliminateDepth();
            business.PictureData = null;
            return Ok(business);
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveBusiness(string password, long id)
        {
            if (id < 1 || string.IsNullOrEmpty(password)) return BadRequest();
            Business business = await repository.Business.FirstOrDefaultAsync(b => b.BusinessID == id);
            if (business == null)
            {
                return NotFound();
            }
            if (business.UserID != claimedId)
            {
                return Unauthorized();
            }
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == claimedId);
            if (new HashCalculator().IsGoodPass(user.Passhash, password))
            {
                await repository.RemoveBusinessAsync(business);
                return Ok();
            }
            return Unauthorized();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> SaveBusiness(BusinessBindingTarget target)
        {
            await repository.SaveBusinessAsync(target.ToBusiness(claimedId));
            return Ok();
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPut]
        public async Task<ActionResult> UpdateBusiness(UpdateBusiness business)
        {
            if (business.Business.BusinessID < 1 || business.Business == null || business.Business.Description.Length < 50 || business.Business.Description.Length > 1000) return BadRequest();
            business.Business.UserID = claimedId;
            business.Business.User = null;
            Business targetBusiness = await repository.Business.Include(b => b.User).FirstOrDefaultAsync(b => b.BusinessID == business.Business.BusinessID);
            if (targetBusiness == null)
            {
                return NotFound();
            }
            User user = targetBusiness.User;
            if (user.UserID != claimedId)
            {
                return Unauthorized();
            }
            if (new HashCalculator().IsGoodPass(user.Passhash, business.Password))
            {
                await repository.UpdateBusinessAsync(business.Business);
                return Ok();
            }
            return Unauthorized();
        }

    }
}
