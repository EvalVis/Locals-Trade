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
            HttpResponseMessage response = await httpClient.PostAsync("/api/User/SignIn", null);
            if (response.StatusCode == HttpStatusCode.OK)
            {
            }

    }
}
