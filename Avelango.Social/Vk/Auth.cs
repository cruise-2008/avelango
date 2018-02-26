using System.Collections.Generic;
using Avelango.Handlers.Http;
using Avelango.Handlers.String;
using Avelango.Models.Accessory;

namespace Avelango.Social.Vk
{
    public class Auth
    {
        private readonly HttpManager _httpManager;
        private readonly string _appId = Resources.Socials.Vkontakte.ApplicationID;
        private readonly string _secureKey = Resources.Socials.Vkontakte.SecureKey;
        private readonly string _redirectUri = Resources.Socials.Vkontakte.RedirectUri;

        private const string Oauth = @"https://oauth.vk.com/";

        public Auth() {
            _httpManager = new HttpManager();
        }

        public OperationResult<string> Authorization() {
            var url = StringManager.BuildString(new List<string> { Oauth, @"authorize?client_id=", _appId, "&scope=PERMISSIONS&redirect_uri=", _redirectUri, "&response_type=code" });
            return _httpManager.Get(url);
        }
    }
}
