using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestAPI.Infrastructure;
using RestAPI.Models;
using RestAPI.Models.Repositories;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        private IServiceRepository repository;
        private IConfiguration configuration;

        public OrderController(IServiceRepository repo, IConfiguration config)
        {
            repository = repo;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<IActionResult> SuggestDelivery(DeliverySuggestion deliverySuggestion)
        {
            if(deliverySuggestion == null || deliverySuggestion.OrderId < 1)
            {
                return BadRequest();
            }
            Order order = await repository.Orders.Include(o => o.User).FirstOrDefaultAsync(o => o.Id == deliverySuggestion.OrderId);
            if(order != null)
            {
                User user = order.User;
                if(user != null)
                {
                    new Mailer(configuration).SendMail(deliverySuggestion.letter, user.Email);
                }
                return NotFound();
            }
            else
            {
                return NotFound();
            }
           // new Mailer(configuration).
            //return Ok();
        }


    }
}
