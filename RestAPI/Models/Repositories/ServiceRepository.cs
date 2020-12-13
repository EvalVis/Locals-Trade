using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace RestAPI.Models.Repositories
{
    public class ServiceRepository : IServiceRepository
    {

        private ServiceDbContext context;

        public IQueryable<User> Users => context.Users;
        public IQueryable<Business> Business => context.Business;

        public IQueryable<Product> Products => context.Products;
        public IQueryable<Feedback> Feedbacks => context.Feedbacks;

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

        public async Task UpdateBusinessAsync(Business targetBusiness, Business business)
        {
            context.Entry(targetBusiness).CurrentValues.SetValues(business);
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

    }
}
