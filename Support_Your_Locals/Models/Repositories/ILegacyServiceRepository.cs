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
    }
}
