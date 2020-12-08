using System.Collections.ObjectModel;
using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public interface IUserBusinessService
    {
        Task<ObservableCollection<Business>> GetBusinesses();
        Task DeleteBusiness(long businessId);
        Task<Business> CreateBusiness();
    }
}
