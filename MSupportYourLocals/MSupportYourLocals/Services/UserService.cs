using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MSupportYourLocals.Models;
using Newtonsoft.Json;

namespace MSupportYourLocals.Services
{
    public class UserService : Service, IUserService
    {

        public async Task<User> GetUser()
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            HttpResponseMessage response = await httpClient.GetAsync("/api/User/Current");
            if (response.StatusCode == HttpStatusCode.OK)
            { 
                var result = await response.Content.ReadAsStringAsync(); 
                User user = JsonConvert.DeserializeObject<User>(result);
                return user;
            }
            return null;
        }

    }
}
