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

        public async Task Login(string email, string password)
        {
            var login = new { Email = email, Password = password };
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/User/SignIn", login);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                tokenService.Token = await response.Content.ReadAsStringAsync();
            }
        }

        public async Task Register(UserBindingTarget target)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/User/SignUp", target);
        }

        public async Task<User> GetUser()
        {
            if (tokenService.Token == null) return null;
            System.Diagnostics.Debug.WriteLine("Yes");
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            HttpResponseMessage response = await httpClient.GetAsync("/api/User/Current");
            System.Diagnostics.Debug.WriteLine(response.StatusCode);
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
