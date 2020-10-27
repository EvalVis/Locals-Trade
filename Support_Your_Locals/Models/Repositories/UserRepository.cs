using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ServiceDbContext context;

        public UserRepository(ServiceDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<User> Users => context.Users;
    }
}
