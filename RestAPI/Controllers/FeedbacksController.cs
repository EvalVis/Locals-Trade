using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Models.BindingTargets;
using RestAPI.Models.Repositories;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbacksController : ControllerBase
    {

        private IServiceRepository repository;

        public FeedbacksController(IServiceRepository repo)
        {
            repository = repo;
        }

        [HttpGet("{businessId}")]
        public IEnumerable<Feedback> GetFeedbacks(long businessId)
        {
            return repository.Feedbacks.Where(f => f.BusinessID == businessId);
        }

        [HttpPost]
        public async Task SaveFeedback(FeedbackBindingTarget feedbackBindingTarget)
        {
            Feedback feedback = feedbackBindingTarget.ToFeedback();
            await repository.AddFeedbackAsync(feedback);
        }

    }
}
