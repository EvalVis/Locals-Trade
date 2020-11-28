using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using RestAPI.Models.Repositories;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [Route("/")]
    [ApiController]
    public class BusinessController : ControllerBase
    {
        private IServiceRepository repository;

        public BusinessController(IServiceRepository repo)
        {
            repository = repo;
        }

        [HttpGet]
        public IEnumerable<Business> GetBusinesses()
        {
            IEnumerable<Business> businesses = repository.Business.
                Include(b => b.User).
                Include(b => b.Products).
                Include(b => b.Workdays);
            foreach (var b in businesses)
            {
                b.User.Passhash = null;
                b.User.Businesses = null;
                foreach (var w in b.Workdays) w.Business = null;
                foreach (var p in b.Products) p.Business = null;
                foreach (var f in b.Feedbacks) f.Business = null;
            }
            return businesses;
        }

        [HttpGet("{id}")]
        public async Task<Business> Business(long id)
        {
            return await repository.Business.FirstOrDefaultAsync(b => b.BusinessID == id);
        }

    }
}
