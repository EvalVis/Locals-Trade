using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MSupportYourLocals.Infrastructure;
using MSupportYourLocals.Models;
using Newtonsoft.Json;

namespace MSupportYourLocals.Services
{
    class BusinessService : Service, IBusinessService
    {

        public async Task<ObservableCollection<Business>> GetBusinesses()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/api/Business/All");
            if (response.StatusCode == HttpStatusCode.OK) 
            {
                var result = await response.Content.ReadAsStringAsync();
                ObservableCollection<Business> businesses = JsonConvert.DeserializeObject<ObservableCollection<Business>>(result);
                businesses = SortByWeekday.Sort(businesses);
                return businesses;
            } 
            return null;
        }

        public async Task<ObservableCollection<Business>> GetUserBusinesses()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            System.Diagnostics.Debug.WriteLine("I am in.");
            HttpResponseMessage response = await httpClient.GetAsync("/api/Business/User");
            System.Diagnostics.Debug.WriteLine("I am out.");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                System.Diagnostics.Debug.WriteLine("I am in. 1");
                var result = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("I am out. 1");
                ObservableCollection<Business> businesses = JsonConvert.DeserializeObject<ObservableCollection<Business>>(result);
                System.Diagnostics.Debug.WriteLine("I am out. 2");
                businesses = SortByWeekday.Sort(businesses);
                System.Diagnostics.Debug.WriteLine(response.StatusCode.ToString() + " the response");
                return businesses;
            }
            return null;
        }

        public Task DeleteBusiness(long businessId)
        {
            return null;
        }

        public async Task CreateBusiness(Business business)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Business", business);
        }

    }
}
