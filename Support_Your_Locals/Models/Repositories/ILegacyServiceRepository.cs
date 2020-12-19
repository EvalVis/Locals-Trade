using System;
using System.Collections.Generic;

namespace Support_Your_Locals.Models.Repositories
{
    public interface ILegacyServiceRepository
    {
        public List<Business> GetBusinesses();

        public User GetUser(long userId);
        public List<User> GetUsers();
        public void DeleteBusiness(long businessId);
        public void DeleteUser(long userId);

        public List<Product> GetProducts();

        public List<Question> GetQuestions();

        public void AddQuestion(string email, string text);

        public void AnswerQuestion(long questionId, string response);
    }
}
