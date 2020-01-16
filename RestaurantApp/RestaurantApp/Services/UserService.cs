using RestaurantApp.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms.Internals;

namespace RestaurantApp.Services
{
    public class UserService
    {
        readonly HttpClient client;

        public UserService()
        {
            client = new HttpClient();
            client.Timeout = new TimeSpan(60000000);
            SetupToken();
            client.BaseAddress = new Uri($"{App.AzureBackendUrl}");
        }

        bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        public async Task<UserAuthModel> Login(UserModel user)
        {
            if (IsConnected)
            {
                var serializedItem = JsonConvert.SerializeObject(user);
                var responseMessage = await client.PostAsync($"api/Users/Login", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                    case 201:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        return await Task.Run(() => JsonConvert.DeserializeObject<UserAuthModel>(json));
                    default:
                        throw new Exception(message: "Error Default");
                }
            }
            return null;
        }

        public AuthenticateModel Authenticate()
        {
            try
            {
                if (IsConnected)
                {
                    var responseMessage = client.PostAsync($"api/Users/Authenticate", null).GetAwaiter().GetResult();
                    var json = responseMessage.Content.ReadAsStringAsync().GetAwaiter().GetResult();
                    return Task.Run(() => JsonConvert.DeserializeObject<AuthenticateModel>(json)).GetAwaiter().GetResult();
                }
                return null;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                return null;
            }
        }

        public void SetupToken()
        {
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("token", ""));
        }
    }
}
