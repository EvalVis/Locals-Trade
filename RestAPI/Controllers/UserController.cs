using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Cryptography;
using RestAPI.Models;
using RestAPI.Models.BindingTargets;
using RestAPI.Models.Repositories;

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

        public UserController(IServiceRepository repo, JsonWebToken token, IHttpContextAccessor accessor)
        {
            claimedId = long.Parse(accessor.HttpContext.User.Claims.FirstOrDefault(type => type.Type == ClaimTypes.NameIdentifier)?.Value ?? "0");
            repository = repo;
            jsonWebToken = token;
        }

        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPatch("email")]
        public async Task<IActionResult> PatchEmail(EmailPatch patch)
        {
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Email, patch.NewEmail);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == claimedId);
            if (user != null)
            {
                if (new HashCalculator().IsGoodPass(user.Passhash, patch.Password))
                {
                    await repository.Patch(document, user);
                    return Ok();
                }
                return Unauthorized();
            }
            return NotFound();
        }

        [HttpPatch("password")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> PatchPassword(PasswordPatch patch)
        {
            string hashed = new HashCalculator().PassHash(patch.NewPassword);
            JsonPatchDocument<User> document = new JsonPatchDocument<User>();
            document.Replace(u => u.Passhash, hashed);
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == claimedId);
            if (user != null)
            {
                if (new HashCalculator().IsGoodPass(user.Passhash, patch.CurrentPassword))
                {
                    await repository.Patch(document, user);
                    return Ok();
                }
                return Unauthorized();
            }
            return NotFound();
        }

        [AllowAnonymous]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [HttpPost("SignUp")]
        public async Task<IActionResult> SignUp(UserBindingTarget target)
        {
            if (target == null) return BadRequest();
            if (!repository.Users.Any(u => u.Email == target.Email)) // TODO: SET EMAIL UNIQUE.
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
        public async Task<IActionResult> SignIn(Login login)
        {
            if (login.Email == null || login.Password == null) return BadRequest();
            User user = await repository.Users.FirstOrDefaultAsync(u => u.Email == login.Email);
            if (user != null)
            {
                bool match = new HashCalculator().IsGoodPass(user.Passhash, login.Password);
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

        [HttpGet("Current")]
        public async Task<IActionResult> GetUser()
        {
            User user = await repository.Users.FirstOrDefaultAsync(u => u.UserID == claimedId);
            user.Passhash = null;
            return Ok(user);
        }

    }
}
