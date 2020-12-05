using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSupportYourLocals.Models.Repositories
{
    public interface ILocalRepository
    {
        event EventHandler<BusinessItem> OnBusinessAdded;

        Task<List<BusinessItem>> GetBusinesses();
        Task AddBusiness(BusinessItem item);
    }
}
