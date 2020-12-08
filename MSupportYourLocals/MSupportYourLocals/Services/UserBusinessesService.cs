using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Models;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MSupportYourLocals.Services
{
    public class UserBusinessesService : Service, IBusinessService
    {

        private JsonWebTokenHolder tokenService = DependencyService.Get<JsonWebTokenHolder>();

        public async Task<ObservableCollection<Business>> GetBusinesses()
        {
            HttpClient httpClient = MakeHttpClient();
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            HttpResponseMessage response = await httpClient.GetAsync("/api/Business/User");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                ObservableCollection<Business> businesses = JsonConvert.DeserializeObject<ObservableCollection<Business>>(result);
                businesses = SortByWeekday.Sort(businesses);
                return businesses;
            }
            return null;
        }

    }
}
