using System.Linq;

namespace Support_Your_Locals.Models.Repositories
{
    public interface IServiceRepository
    {
        public IQueryable<Business> Business { get; }
        public IQueryable<TimeSheet> TimeSheets { get; }
    }
}
