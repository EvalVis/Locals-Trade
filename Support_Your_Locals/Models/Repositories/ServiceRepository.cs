using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Support_Your_Locals.Models.Repositories
{
    public class ServiceRepository : IServiceRepository
    {

        private ServiceDbContext context;

        public IQueryable<User> Users => context.Users;
        public IQueryable<Business> Business => context.Business;

        public IQueryable<Product> Products => context.Products;

        public ServiceRepository(ServiceDbContext ctx)
        {
            context = ctx;
        }

        public void AddUser(User user)
        {
            context.Add(user);
            context.SaveChanges();
        }

        public void AddBusiness(Business business)
        {
            context.Add(business);
            context.SaveChanges();
        }

        public void AddFeedback(Feedback feedback)
        {
            context.Add(feedback);
            context.SaveChanges();
        }

        public void DeleteBusiness(Business business)
        {
            context.Remove(business);
            context.SaveChanges();
        }

        public void SaveBusiness(Business business)
        {
            context.SaveChanges();
        }

    }
}
