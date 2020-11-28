using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Models.Repositories;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
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

    }
}
