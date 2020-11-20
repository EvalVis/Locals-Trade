using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals.Pages.User
{
    [Authorize]
    public class UserSettingsModel : PageModel
    {

        private HttpContext context;
        private IServiceRepository repository;

        public Models.User User { get; set; }

        public UserSettingsModel(IHttpContextAccessor accessor, IServiceRepository repo)
        {
            context = accessor.HttpContext;
            repository = repo;
        }

        public async Task OnGetAsync()
        {
            long userID = long.Parse(context.User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
            User = await repository.Users.FirstOrDefaultAsync(u => u.UserID == userID);
        }

    }
}
