using _29Lifts.Api.Public.models;
using _29Lifts.Api.Rides.Models;
using _29Lifts.Exceptions;
using _29Lifts.Services.SettingsServices;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Windows.System;

namespace _29Lifts.Helper
{
    public static class HttpClientHelper
    {
        private static readonly string _clientToken = "XXXXXXX";
#if DEBUG
        private static readonly string _secret = "XXXXX";
#endif
#if !DEBUG
        
#endif
        private static readonly string _clientId = "XXXXXXX";
        private static readonly string _apiAuthEndpoint = "https://api.lyft.com/oauth/token";
        private static SettingsService _settings = SettingsService.Instance;

        public async static Task<HttpClient> GetPublicHttpClient()
        {
            var settingsService = SettingsService.Instance;
            var _client = new HttpClient();

            if (_settings.PublicToken != null && _settings.PublicTokenExpiration > DateTime.Now)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.PublicToken.Token);
                return _client;
            }
            else
            {
                await GetNewPublicAccessToken();
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.PublicToken.Token);
                return _client;
            }

        }

        public async static Task GetNewPublicAccessToken()
        {
            var _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_clientId}:{_secret}")));

            var scopeJson = $"{{\"grant_type\": \"client_credentials\", \"scope\": \"public\"}}";
            var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_apiAuthEndpoint, content);
            var responseText = await response.Content.ReadAsStringAsync();
            var accessToken = JsonConvert.DeserializeObject<AccessTokenPublic>(responseText);

            _settings.PublicToken = accessToken;
            _settings.PublicTokenExpiration = DateTime.Now + TimeSpan.FromSeconds(accessToken.ExpiresIn - 120);
        }

        public async static Task<HttpClient> GetUserHttpClient()
        {
            var settingsService = SettingsService.Instance;
            var _client = new HttpClient();

            if (_settings.UserToken != null && _settings.UserTokenExpiration > DateTime.Now)
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.UserToken.Token);
                Debug.WriteLine($"using old token: {_settings.UserToken.Token}");
                return _client;
            }
            else
            {
                await GetNewUserAccessToken();
                _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _settings.UserToken.Token);
                Debug.WriteLine($"created new token: {_settings.UserToken.Token}");
                return _client;
            }

        }

        // todo
        public async static Task GetNewUserAccessToken()
        {
            if (_settings.UserToken == null)
            {
                throw new NotLoggedInException();
            }

            var _client = new HttpClient();
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_clientId}:{_secret}")));
            var refreshToken = _settings.UserToken.RefreshToken;


            var scopeJson = $"{{\"grant_type\": \"refresh_token\", \"refresh_token\": \"{refreshToken}\"}}";
            var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");

            var response = await _client.PostAsync(_apiAuthEndpoint, content);
            var responseText = await response.Content.ReadAsStringAsync();
            var settings = new JsonSerializerSettings() { MissingMemberHandling = MissingMemberHandling.Ignore };
            var userToken = JsonConvert.DeserializeObject<AccessTokenPublic>(responseText, settings);

            Debug.WriteLine($"created a new token: {userToken.Token}");

            _settings.UserToken.Token = userToken.Token;
            _settings.UserToken.TokenType = userToken.TokenType;
            _settings.UserToken.Scope = userToken.Scope;
            _settings.UserTokenExpiration = DateTime.Now + TimeSpan.FromSeconds(userToken.ExpiresIn - 120);

        }


        /// <summary>
        /// Only used when the user has signed in or signed up through Lyft's website.
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        public async static Task<bool> AuthenticateWithCode(string code)
        {
            try
            {
                var _client = new HttpClient();
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.ASCII.GetBytes($"{_clientId}:{_secret}")));

                var scopeJson = $"{{\"grant_type\": \"authorization_code\", \"code\": \"{code}\"}}";
                var content = new StringContent(scopeJson, Encoding.UTF8, "application/json");

                var response = await _client.PostAsync(_apiAuthEndpoint, content);
                var responseText = await response.Content.ReadAsStringAsync();
                var userToken = JsonConvert.DeserializeObject<AccessTokenAuth>(responseText);

                _settings.UserToken = userToken;
                _settings.UserTokenExpiration = DateTime.Now + TimeSpan.FromSeconds(userToken.ExpiresIn - 120);

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public static void RegisterLoginAtLyft()
        {
            var guid = Guid.NewGuid().ToString();
            var uri = new Uri($"https://api.lyft.com/oauth/authorize?response_type=code&client_id=85X7D3CKRnHD&scope=public rides.read offline rides.request profile&state={guid}");
            Task.Run(() => Launcher.LaunchUriAsync(uri));
        }


    }

}
