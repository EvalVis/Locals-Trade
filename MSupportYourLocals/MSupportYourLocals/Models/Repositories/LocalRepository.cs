using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MSupportYourLocals.Models.Repositories
{
    public class LocalRepository : ILocalRepository
    {
        public event EventHandler<BusinessItem> OnBusinessAdded;

        public Task<List<BusinessItem>> GetBusinesses()
        {
            return null;
        }

        public Task AddBusiness(BusinessItem item)
        {
            return null;
        }

    }
}
