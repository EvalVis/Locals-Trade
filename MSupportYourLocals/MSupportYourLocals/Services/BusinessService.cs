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

        public async Task<PageBusiness> GetBusinesses()
        {
            HttpResponseMessage response = await httpClient.GetAsync("/api/Business/All");
            if (response.StatusCode == HttpStatusCode.OK) 
            {
                var result = await response.Content.ReadAsStringAsync();
                System.Diagnostics.Debug.WriteLine("the string " + result);
                PageBusiness pageBusiness = JsonConvert.DeserializeObject<PageBusiness>(result);
                System.Diagnostics.Debug.WriteLine(pageBusiness.TotalPages, pageBusiness.Businesses[0].Header);
                return pageBusiness;
            } 
            return null;
        }

        public async Task<ObservableCollection<Business>> GetUserBusinesses()
        {
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

        public async Task DeleteBusiness(string password, long businessId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            HttpResponseMessage response = await httpClient.DeleteAsync($"/api/Business/{businessId}?password={password}");
        }

        public async Task CreateBusiness(Business business)
        {
            var businessBindingTarget = new
            {
                Description = business.Description, Header = business.Header, PhoneNumber = business.PhoneNumber,
                Longitude = business.Longitude, Latitude = business.Latitude, WorkdayTargets = business.Workdays, ProductTargets = business.Products
            };
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/Business", businessBindingTarget);
        }

        public async Task UpdateBusiness(string password, Business business)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            var updateBusiness = new {Password = password, Business = business};
            HttpResponseMessage response = await httpClient.PutAsJsonAsync("/api/Business", updateBusiness);
        }

    }
}
