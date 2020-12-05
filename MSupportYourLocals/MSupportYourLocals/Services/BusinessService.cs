using System;
using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MSupportYourLocals.Models;
using Newtonsoft.Json;

namespace MSupportYourLocals.Services
{
    class BusinessService : IBusinessService
    {

        public async Task<ObservableCollection<Business>> GetBusinesses()
        {
            using (HttpClient httpClient = new HttpClient())
            {
                httpClient.BaseAddress = new Uri("https://10.0.2.2:44311");
                httpClient.DefaultRequestHeaders.Accept.Clear();
                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                System.Diagnostics.Debug.WriteLine("Testavimas 3");
                HttpResponseMessage response = await httpClient.GetAsync("/api/Business/All");
                System.Diagnostics.Debug.WriteLine("Testavimas 4");
                if (response.StatusCode == HttpStatusCode.OK)
                {
                    System.Diagnostics.Debug.WriteLine("Testavimas 5");
                    var result = await response.Content.ReadAsStringAsync();
                    System.Diagnostics.Debug.WriteLine("Testavimas 6");
                    ObservableCollection<Business> businesses =
                        JsonConvert.DeserializeObject<ObservableCollection<Business>>(result);
                    return businesses;
                }
                return null;
            }
        }

    }
}
