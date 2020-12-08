using System.Collections.ObjectModel;
using System.Net.Http;
using System.Threading.Tasks;
using MSupportYourLocals.Models;

namespace MSupportYourLocals.Services
{
    public class UserBusinessesService : Service, IBusinessService
    {

        public Task<ObservableCollection<Business>> GetBusinesses()
        {
            HttpClient httpClient = MakeHttpClient();
            HttpResponseMessage response = await httpClient.GetAsync()
        }

    }
}
