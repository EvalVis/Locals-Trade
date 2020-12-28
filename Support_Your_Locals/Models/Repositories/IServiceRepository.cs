using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public interface IServiceRepository
    {
        public IQueryable<User> Users { get; }
        public IQueryable<Business> Business { get; }
        public IQueryable<Product> Products { get; }
        public IQueryable<Order> Orders { get; }
        public void AddUser(User user);
        public void AddBusiness(Business business);
        public void AddFeedback(Feedback feedback);
        public void SaveBusiness(Business business);
        public void DeleteBusiness(Business business);
        public void SaveUser(User user);
        public void DeleteUser(User user);
        public void AddOrder(Order order);
        public void RemoveOrder(Order order);
    }
}
