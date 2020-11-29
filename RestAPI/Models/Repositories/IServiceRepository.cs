﻿using System.Linq;
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
        public void AddUser(User user);
        public void AddBusiness(Business business);
        public void AddFeedback(Feedback feedback);
        public Task SaveFeedbackAsync(Feedback feedback);
        public Task Patch<T>(JsonPatchDocument<T> document, T entity) where T: class;
        public Task SaveBusinessAsync(Business business);
        public Task RemoveBusinessAsync(Business business);
        public void SaveUser(User user);
    }
}
