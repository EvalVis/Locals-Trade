using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using MSupportYourLocals.Infrastructure.Extensions;
using MSupportYourLocals.Models;
using Newtonsoft.Json;

namespace MSupportYourLocals.Services
{
    public class UserService : Service, IUserService
    {

        public async Task<bool> Login(string email, string password)
        {
            var login = new { Email = email, Password = password };
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/User/SignIn", login);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                tokenService.Token = await response.Content.ReadAsStringAsync();
                return true;
            }
            return false;
        }

        public async Task<bool> Register(UserBindingTarget target)
        {
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/User/SignUp", target);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<User> GetUser()
        {
            if (tokenService.Token == null) return null;
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

        public async Task<bool> PatchPassword(string currentPassword, string newPassword)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            var passwordPatch = new { CurrentPassword = currentPassword, NewPassword = newPassword };
            HttpResponseMessage response = await httpClient.PatchAsync("/api/User/password", passwordPatch);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

        public async Task<bool> PatchEmail(string password, string email)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", tokenService.Token);
            var emailPatch = new {Password = password, NewEmail = email};
            HttpResponseMessage response = await httpClient.PatchAsync("/api/User/email", emailPatch);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            return false;
        }

    }
}
