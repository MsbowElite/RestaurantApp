using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using Xamarin.Essentials;

namespace RestaurantApp.Services
{
    public abstract class DataStore<T>
    {
        protected HttpClient _client;
        protected IEnumerable<T> _items;
        protected readonly string _endPoint;
        protected bool IsConnected => Connectivity.NetworkAccess == NetworkAccess.Internet;

        protected DataStore(string endPoint)
        {
            _endPoint = endPoint;
        }

        protected string MessageStatus409 = "Item duplicado, nome repetido!";
        protected string MessageDefault = "Error no servidor!";

        protected void HandleResponseCode(int code)
        {
            switch (code)
            {
                case 200:
                case 201:
                    return;
                case 409:
                    throw new Exception(message: MessageStatus409);
                default:
                    throw new Exception(message: MessageDefault);
            }
        }

        public void SetupToken()
        {
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", Preferences.Get("token", ""));
        }
    }
}
