using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;

namespace RestAPI.Models.Repositories
{
    public class ServiceRepository : IServiceRepository
    {

        private ServiceDbContext context;

        public IQueryable<User> Users => context.Users;
        public IQueryable<Business> Business => context.Business;
        public IQueryable<TimeSheet> Workdays => context.TimeSheets;

        public IQueryable<Product> Products => context.Products;
        public IQueryable<Feedback> Feedbacks => context.Feedbacks;
        public IQueryable<Question> Questions => context.Questions;
        public IQueryable<Order> Orders => context.Orders;

        public ServiceRepository(ServiceDbContext ctx)
        {
            context = ctx;
        }

        public async Task SaveFeedbackAsync(Feedback feedback)
        {
            await context.Feedbacks.AddAsync(feedback);
            await context.SaveChangesAsync();
        }

        public async Task RemoveBusinessAsync(Business business)
        {
            context.Remove(business);
            await context.SaveChangesAsync();
        }

        public async Task SaveBusinessAsync(Business business)
        {
            await context.Business.AddAsync(business);
            await context.SaveChangesAsync();
        }

        public async Task SaveUserAsync(User user)
        {
            await context.AddAsync(user);
            await context.SaveChangesAsync();
        }

        public async Task Patch<T>(JsonPatchDocument<T> document, T entity) where T : class
        {
            document.ApplyTo(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateBusinessAsync(Business business)
        {
            Business dbBusiness = await context.Business.Include(w => w.Workdays).FirstOrDefaultAsync(b => b.BusinessID == business.BusinessID);
            if (dbBusiness == null) return;
            dbBusiness.Description = business.Description;
            dbBusiness.Longitude = business.Longitude;
            dbBusiness.Latitude = business.Latitude;
            dbBusiness.PhoneNumber = business.PhoneNumber;
            dbBusiness.Header = business.Header;
            foreach(var w in dbBusiness.Workdays)
            {
                context.TimeSheets.Remove(w);
            }
            await context.SaveChangesAsync();
            foreach(var w in business.Workdays)
            {
                w.TimeSheetID = 0;
            }
            dbBusiness.Workdays = business.Workdays;
            await context.SaveChangesAsync();
        }

        public async Task RemoveFeedbacksAsync(IEnumerable<Feedback> feedbacks)
        {
            context.RemoveRange(feedbacks);
            await context.SaveChangesAsync();
        }

        public async Task RemoveFeedbackAsync(Feedback feedback)
        {
            context.Remove(feedback);
            await context.SaveChangesAsync();
        }

        public async Task AskForHelpAsync(Question question)
        {
            await context.Questions.AddAsync(question);
            await context.SaveChangesAsync();
        }

    }
}
