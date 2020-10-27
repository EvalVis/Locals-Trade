using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public class ServiceRepositoryDb : IServiceRepository
    {

        private ServiceDbContext context;

        public ServiceRepositoryDb(ServiceDbContext ctx)
        {
            context = ctx;
        }

        public IQueryable<Business> Business => context.Business;
        public IQueryable<TimeSheet> TimeSheets => context.TimeSheets;
    }
}
