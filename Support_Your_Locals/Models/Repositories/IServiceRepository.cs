using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Support_Your_Locals.Models.Repositories
{
    public interface IServiceRepository
    {

        public IQueryable<User> Users { get;}
        public IQueryable<Business> Business { get; }
        public IQueryable<TimeSheet> TimeSheets { get; }
    }
}
