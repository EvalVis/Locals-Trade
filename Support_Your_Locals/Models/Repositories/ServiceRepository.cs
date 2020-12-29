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
        public IQueryable<Order> Orders => context.Orders;

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

        public void UpdateBusiness(Business business)
        {
            Business dbBusiness = context.Business.Include(w => w.Workdays).FirstOrDefault(b => b.BusinessID == business.BusinessID);
            if (dbBusiness == null) return;
            dbBusiness.Description = business.Description;
            dbBusiness.Longitude = business.Longitude;
            dbBusiness.Latitude = business.Latitude;
            dbBusiness.PhoneNumber = business.PhoneNumber;
            dbBusiness.Header = business.Header;
            foreach (var w in dbBusiness.Workdays)
            {
                context.TimeSheets.Remove(w);
            }
            context.SaveChanges();
            foreach (var w in business.Workdays)
            {
                w.TimeSheetID = 0;
            }
            dbBusiness.Workdays = business.Workdays;
            context.SaveChanges();
        }

        public void SaveUser(User user)
        {
            context.SaveChanges();
        }

        public void DeleteUser(User user)
        {
            context.Remove(user);
            context.SaveChanges();
        }

        public void AddOrder(Order order)
        {
            context.Add(order);
            context.SaveChanges();
        }

        public void RemoveOrder(Order order)
        {
            context.Remove(order);
            context.SaveChanges();
        }

    }
}
