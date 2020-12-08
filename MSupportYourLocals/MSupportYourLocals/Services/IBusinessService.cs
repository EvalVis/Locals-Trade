using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IBusinessService
    {
        Task<ObservableCollection<Business>> GetBusinesses();
        Task<ObservableCollection<Business>> GetUserBusinesses();
        Task DeleteBusiness(long businessId);
        Task CreateBusiness(Business business);
    }
}
