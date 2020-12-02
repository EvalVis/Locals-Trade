using System.Linq;
using System.Security.Claims;
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
        private long claimedId;

        public UserController(IServiceRepository repo, JsonWebToken token)
        {
            claimedId = long.Parse(HttpContext.User.Claims.FirstOrDefault(type => type.Value == ClaimTypes.NameIdentifier).Value);
            repository = repo;
            jsonWebToken = token;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [HttpPatch("email/{email}")]
        public async Task<IActionResult> PatchEmail(string password, string newEmail)
        {
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Email, newEmail);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == claimedId);
            if (user != null)
            {
                if (new HashCalculator().IsGoodPass(user.Passhash, password))
                {
                    await repository.Patch(document, user);
                    return Ok();
                }
                return Unauthorized();
            }
            return NotFound();
        }

        [HttpPatch("password/{email}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> PatchPassword(string currentPassword, string newPassword)
        {
            string hashed = new HashCalculator().PassHash(newPassword);
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Passhash, hashed);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == claimedId);
            if (user != null)
            {
                if (new HashCalculator().IsGoodPass(user.Passhash, currentPassword))
                {
                    await repository.Patch(document, user);
                    return Ok();
                }
                return Unauthorized();
            }
            return NotFound();
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserBindingTarget target)
        {
            if (!repository.Users.Any(u => u.Email == target.Email))
            {
                await repository.SaveUserAsync(target.ToUser());
                return Ok();
            }
            return Conflict();
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
