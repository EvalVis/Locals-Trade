using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public interface IServiceRepository
    {
        public IQueryable<User> Users { get; }
        public IQueryable<Business> Business { get; }
        public IQueryable<Product> Products { get; }
        public void AddUser(User user);
        public void AddBusiness(Business business);
    }
}
