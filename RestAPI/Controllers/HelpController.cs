using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestAPI.Models;
using RestAPI.Models.Repositories;
using Support_Your_Locals.Models;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class HelpController : ControllerBase
    {

        private IServiceRepository repository;

        public HelpController(IServiceRepository repo)
        {
            repository = repo;
        }

        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost]
        public async Task<ActionResult> Ask(HelpMessage helpMessage)
        {
            Question question = helpMessage.ToQuestion();
            await repository.AskForHelpAsync(question);
            return Ok();
        }
    }
}
