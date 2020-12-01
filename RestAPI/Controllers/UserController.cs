using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using RestAPI.Models.Repositories;
using RestAPI.Cryptography;
using RestAPI.Models.BindingTargets;

namespace RestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private IServiceRepository repository;
        private JsonWebToken jsonWebToken;

        public UserController(IServiceRepository repo, JsonWebToken token)
        {
            repository = repo;
            jsonWebToken = token;
        }

        [HttpPatch("email/{id}")]
        public async Task PatchEmail(long id, string email)
        {
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Email, email);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (user != null)
            {
                await repository.Patch(document, user);
            }
        }

        [HttpPatch("password/{id}")]
        public async Task PatchPassword(long id, string password)
        {
            string hashed = new HashCalculator().PassHash(password);
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Passhash, hashed);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == id);
            if (User != null)
            {
                await repository.Patch(document, user);
            }
        }

        /*[HttpPost]
        public async Task SignUp(UserBindingTarget target)
        {
            if (!repository.Users.Any(u => u.Email == target.Email))
            {
                await repository.SaveUserAsync(target.ToUser());
            }
        }*/

        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            if (email == null || password == null) return BadRequest();
            string passwordHash = new HashCalculator().PassHash(password);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null && user.Passhash == passwordHash)
            {
                jsonWebToken.Authenticate(email, password);
                if (jsonWebToken == null)
                {
                    return Unauthorized();
                }
                return Ok(jsonWebToken);
            }
            return NotFound();
        }

    }
}
