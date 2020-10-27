using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public class UserRepository : IUserRepository
    {
        private ServiceDbContext context;
        public IQueryable<User> Users => context.Users;

        public UserRepository(ServiceDbContext ctx)
        {
            context = ctx;
        }

        public void Add(User user)
        {
            context.Add(user);
            context.SaveChanges();
        }
    }
}
