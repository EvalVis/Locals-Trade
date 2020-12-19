using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace MSupportYourLocals.Infrastructure.Extensions
{
    public static class HttpClientExtensions
    {

        public static async Task<HttpResponseMessage> PatchAsync(this HttpClient client, string requestUri, Object data)
        {
            StringContent content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var method = new HttpMethod("PATCH");
            var request = new HttpRequestMessage(method, requestUri)
            {
                Content = content
            };
            return await client.SendAsync(request);
        }

    }
}
