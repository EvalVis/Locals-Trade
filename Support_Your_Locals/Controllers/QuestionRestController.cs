using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals.Controllers
{
    [Route("api/questions")]
    [ApiController]
    public class QuestionRestController : ControllerBase
    {

        private ILegacyServiceRepository repository;

        public QuestionRestController(ILegacyServiceRepository repo)
        {
            repository = repo;

        }
       
        [HttpPost]
        public IActionResult Post([FromBody] AddQuestionResource question)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }
           repository.AddQuestion(question.Email, question.Text);

           return Ok();
        }
    }

    public class AddQuestionResource
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
