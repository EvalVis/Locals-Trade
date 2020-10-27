using System.Collections.Generic;
using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public interface IServiceRepository
    {
        public IQueryable<User> Users { get; }
        public IQueryable<Business> Business { get; }
        public IQueryable<TimeSheet> TimeSheets { get; }
        public void AddUser(User user);
        public void AddBusiness(Business business);
        public void AddTimeSheets(IEnumerable<TimeSheet> timeSheets);
    }
}
