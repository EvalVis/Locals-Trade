using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Adapters.Facebook;

namespace LocalsTradeBot.Controllers
{
    [Route("api/facebook")]
    [ApiController]
    public class FacebookController : ControllerBase
    {
        private readonly FacebookAdapter _adapter;
        private readonly IBot _bot;

        public FacebookController(FacebookAdapter adapter, IBot bot)
        {
            _adapter = adapter;
            _bot = bot;
        }

        [HttpPost]
        [HttpGet]
        public async Task PostAsync()
        {
            await _adapter.ProcessAsync(Request, Response, _bot);
        }
    }
}
