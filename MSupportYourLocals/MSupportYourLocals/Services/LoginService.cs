using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace MSupportYourLocals.Services
{
    public class LoginService : Service, ILoginService
    {

        public async Task Login(string email, string password)
        {
            HttpClient httpClient = MakeHttpClient();
            var login = new {Email = email, Password = password};
            HttpResponseMessage response = await httpClient.PostAsJsonAsync("/api/User/SignIn", login);
            if (response.StatusCode == HttpStatusCode.OK)
            {
                System.Diagnostics.Debug.WriteLine(await response.Content.ReadAsStringAsync());
            }
        }

    }
}
