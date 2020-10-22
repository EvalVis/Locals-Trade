using System;
using System.Collections.Generic;
using System.Linq;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals.Models
{
    public class SearchBusiness
    {

        private IServiceRepository repository;

        public SearchBusiness(IServiceRepository repo)
        {
            repository = repo;
        }

        public IEnumerable<Business> SearchByHeaderIgnoreCase(String header)
        {
            return repository.Business.Where(b => b.Header.ToLower().Equals(header.ToLower()));
        }

        public IEnumerable<Business> SearchByDescriptionIgnoreCase(String description)
        {
            return repository.Business.Where(b => b.Description.ToLower().Equals(description.ToLower()));
        }

        //TODO: Search by working hours.

        public IEnumerable<Business> SearchByWeekday(int weekday)
        {
            IEnumerable<TimeSheet> timeSheets = repository.TimeSheets.Where(t => t.Weekday == weekday);
            foreach(var t in timeSheets)
            {
                yield return repository.Business.FirstOrDefault(b => b.BusinessID == t.BusinessID);
            }
        }

        public IEnumerable<Business> SearchByOwnersSurname(string surname)
        {
            IEnumerable<User> users = repository.Users.Where(u => u.Surname == surname);
            foreach (var u in users)
            {
                yield return repository.Business.FirstOrDefault(b => b.UserID == u.UserID);
            }
        }

    }
}
