using System.Collections.Generic;
using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public class ServiceRepositoryDb : IServiceRepository
    {

        private ServiceDbContext context;

        public IQueryable<User> Users => context.Users;
        public IQueryable<Business> Business => context.Business;
        public IQueryable<TimeSheet> TimeSheets => context.TimeSheets;

        public ServiceRepositoryDb(ServiceDbContext ctx)
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

        public void AddTimeSheet(TimeSheet timeSheet)
        {
            context.Add(timeSheet);
            context.SaveChanges();
        }

        public void AddTimeSheets(IEnumerable<TimeSheet> timeSheets)
        {
            context.AddRange(timeSheets);
            context.SaveChanges();
        }

    }
}
