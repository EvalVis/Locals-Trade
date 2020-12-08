using System.Collections.ObjectModel;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MSupportYourLocals.Models;
using Newtonsoft.Json;

namespace MSupportYourLocals.Services
{
    public class FeedbackService : Service, IFeedbackService
    {

        public async Task<ObservableCollection<Feedback>> GetFeedbacks(long businessId)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            HttpResponseMessage response = await httpClient.GetAsync($"/api/Feedback/{businessId}");
            if (response.StatusCode == HttpStatusCode.OK)
            {
                var result = await response.Content.ReadAsStringAsync();
                ObservableCollection<Feedback> feedbacks = JsonConvert.DeserializeObject<ObservableCollection<Feedback>>(result);
                return feedbacks;
            }
            return null;
        }

    }
}
