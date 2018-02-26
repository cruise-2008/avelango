using System.Collections.Generic;
using Avelango.Handlers.Http;

namespace Avelango.Handlers.Api.LiqPay
{
    public class LiqPayManager
    {
        private readonly HttpManager _httpManager;

        public LiqPayManager()
        {
            _httpManager = new HttpManager();
        }


        public void Pay() {
            var result = _httpManager.Post("", new Dictionary<string, string>());
        }
    }
}
