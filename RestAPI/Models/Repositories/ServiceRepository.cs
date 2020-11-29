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

        public void SaveUser(User user)
        {
            context.SaveChanges();
        }

        public async Task Patch<T> (JsonPatchDocument<T> document, T entity) where T: class
        {
            document.ApplyTo(entity);
            await context.SaveChangesAsync();
        }

        public async Task UpdateBusinessAsync(Business business)
        {
            context.Update(business);
            await context.SaveChangesAsync();
        }

    }
}
