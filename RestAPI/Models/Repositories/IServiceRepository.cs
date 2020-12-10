using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch;

namespace RestAPI.Models.Repositories
{
    public interface IServiceRepository
    {
        public IQueryable<User> Users { get; }
        public IQueryable<Business> Business { get; }
        public IQueryable<Product> Products { get; }
        public IQueryable<Feedback> Feedbacks { get; }
        public Task SaveFeedbackAsync(Feedback feedback);
        public Task Patch<T>(JsonPatchDocument<T> document, T entity) where T: class;
        public Task SaveBusinessAsync(Business business);
        public Task RemoveBusinessAsync(Business business);
        public Task UpdateBusinessAsync(Business business);
        public Task SaveUserAsync(User user);
        public Task RemoveFeedbacksAsync(IEnumerable<Feedback> feedbacks);
        public Task RemoveFeedbackAsync(Feedback feedback);
    }
}
