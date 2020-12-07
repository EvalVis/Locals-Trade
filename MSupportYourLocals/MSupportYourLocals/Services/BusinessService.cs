using System;
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
            #if DEBUG
            HttpClientHandler insecureHandler = GetInsecureHandler();
            HttpClient httpClient = new HttpClient(insecureHandler);
            #else
            HttpClient client = new HttpClient();
            #endif
            httpClient.BaseAddress = new Uri("https://10.0.2.2:44311/"); 
            httpClient.DefaultRequestHeaders.Accept.Clear(); 
            httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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

    }
}
