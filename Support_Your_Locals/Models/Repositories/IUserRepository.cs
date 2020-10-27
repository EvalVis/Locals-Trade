using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public interface IUserRepository
    {
        public IQueryable<User> Users { get; }
    }
}
