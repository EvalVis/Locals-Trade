using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
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
    public class FeedbackController : ControllerBase
    {

        private IServiceRepository repository;
        private long claimedId;

        public FeedbackController(IServiceRepository repo)
        {
            claimedId = long.Parse(HttpContext.User.Claims.FirstOrDefault(type => type.Value == ClaimTypes.NameIdentifier)?.Value ?? "0");
            repository = repo;
        }

        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpGet("{businessId}")]
        public async Task<ActionResult<IEnumerable<Feedback>>> GetFeedbacks(long businessId)
        {
            if (businessId < 1) return BadRequest();
            Business business = await repository.Business.FirstOrDefaultAsync(b => b.BusinessID == businessId);
            if (business == null)
            {
                return NotFound();
            }
            if (business.UserID == claimedId)
            {
                IEnumerable<Feedback> feedbacks = repository.Feedbacks.Where(f => f.BusinessID == businessId);
                if (!feedbacks.Any()) return NoContent();
                return Ok(feedbacks);
            }
            return Unauthorized();
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> SaveFeedback(FeedbackBindingTarget feedbackBindingTarget)
        {
            Feedback feedback = feedbackBindingTarget.ToFeedback();
            await repository.SaveFeedbackAsync(feedback);
            return Ok();
        }

    }
}
