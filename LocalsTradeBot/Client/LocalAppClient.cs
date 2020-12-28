using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace LocalsTradeBot.Client
{
    public class LocalAppClient
    {
        private static readonly HttpClient client = new HttpClient();
        private static readonly string AppUrl = "https://supportyourlocals20201220224514.azurewebsites.net/";
        private static readonly string QuestionsApiPath = "api/questions"; 

        public Task CreateQuestion(string email, string question)
        {
            var body = $"\"Email\":\"${email}\",\"Question\":\"${question}\"";
            var content = new StringContent(body, Encoding.UTF8, "application/json");
            return client.PostAsync(AppUrl + QuestionsApiPath, content);
        }
        
    }
}
