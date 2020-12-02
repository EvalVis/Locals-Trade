using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
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

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPatch("email/{email}")]
        public async Task<IActionResult> PatchEmail(string email, string newEmail)
        {
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Email, newEmail);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                await repository.Patch(document, user);
                return Ok();
            }
            return NotFound();
        }
        [HttpPatch("password/{email}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> PatchPassword(string email, string password)
        {
            string hashed = new HashCalculator().PassHash(password);
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Passhash, hashed);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            {
                await repository.Patch(document, user);
                return Ok();
            }
            return NotFound();
        }

        [AllowAnonymous]
        [HttpPost("SignUp")]
        public async Task SignUp(UserBindingTarget target)
        {
            if (!repository.Users.Any(u => u.Email == target.Email))
            {
                await repository.SaveUserAsync(target.ToUser());
            }
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpPost("SignIn")]
        public async Task<IActionResult> SignIn(string email, string password)
        {
            if (email == null || password == null) return BadRequest();
            User user = await repository.Users.FirstOrDefaultAsync(u => u.Email == email);
            if (user != null)
            { 
                bool match = new HashCalculator().IsGoodPass(user.Passhash, password); 
                if (match) 
                { 
                    var token = jsonWebToken.Authenticate(user.UserID);
                    if (jsonWebToken == null)
                    {
                        return Unauthorized();
                    }
                    return Ok(token);
                }
                return Unauthorized();
            }
            return NotFound();
        }

    }
}
