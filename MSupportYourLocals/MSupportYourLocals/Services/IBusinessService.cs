using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IBusinessService
    {
        Task<ObservableCollection<Business>> GetBusinesses();
        Task<ObservableCollection<Business>> GetUserBusinesses();
        Task DeleteBusiness(string password, long businessId);
        Task CreateBusiness(Business business);
        Task UpdateBusiness(string password, Business business);
    }
}
