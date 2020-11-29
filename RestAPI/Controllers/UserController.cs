using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestAPI.Models;
using RestAPI.Models.Repositories;
using Support_Your_Locals.Cryptography;

namespace RestAPI.Controllers
{
    public class UserController : ControllerBase
    {
        private IServiceRepository repository;

        public UserController(IServiceRepository repo)
        {
            repository = repo;
        }

        [HttpPatch("{id}")]
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

        [HttpPatch("{id}")]
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

    }
}
