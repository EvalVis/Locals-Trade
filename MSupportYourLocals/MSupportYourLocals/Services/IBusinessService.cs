using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IBusinessService
    {
        Task<PageBusiness> GetBusinesses(int page);
        Task<PageBusiness> GetFilteredBusinesses(string ownersSurname, string businessInfo, int searchIn, bool[] weekdaySelected, DateTime openFrom, DateTime openTo, int page);
        Task<ObservableCollection<Business>> GetUserBusinesses();
        Task<bool> DeleteBusiness(string password, long businessId);
        Task<bool> CreateBusiness(Business business);
        Task<bool> UpdateBusiness(string password, Business business);
    }
}
