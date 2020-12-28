using Microsoft.AspNetCore.Mvc;
using RestAPI.Models.Repositories;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController
    {

        private IServiceRepository repository;

        public OrderController(IServiceRepository repo)
        {
            repository = repo;
        }


    }
}
