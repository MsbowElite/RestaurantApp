using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Xamarin.Essentials;
using RestaurantApp.Models;
using System.Net.Http.Headers;
using RestaurantApp.Helpers;
using RestaurantApp.Services.Interfaces;

namespace RestaurantApp.Services
{
    public class DishDataStore : DataStore<Dish>, IDishDataStore
    {
        public DishDataStore() : base("api/Dishes")
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.AzureBackendUrl}");
            SetupToken();

            _items = new List<Dish>();
        }

        public async Task<IEnumerable<Dish>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                var responseMessage = await _client.GetAsync(_endPoint);
                HandleResponseCode((int)responseMessage.StatusCode);

                var json = await responseMessage.Content.ReadAsStringAsync();
                return _items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Dish>>(json));
            }
            return _items;
        }

        public async Task<Dish> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                var json = await _client.GetStringAsync($"{_endPoint}/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Dish>(json));
            }

            return null;
        }

        public async Task<Dish> AddItemAsync(Dish item)
        {
            if (item == null)
                throw new Exception(message: "Prato faltando valores");
            if(!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            var serializedItem = JsonConvert.SerializeObject(item);
            var response = await _client.PostAsync($"{_endPoint}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            HandleResponseCode((int)response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<Dish>(json));
        }

        public async Task<bool> UpdateItemAsync(Dish item)
        {
            if (item == null || item.Id == null || !IsConnected)
                throw new Exception(message: "Prato faltando valores");
            if (!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await _client.PutAsync($"{_endPoint}/{item.Id.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            var response = await _client.DeleteAsync($"{_endPoint}/{id}");

            return response.IsSuccessStatusCode;
        }

        private void HandleResponseCode(int code)
        {
            switch (code)
            {
                case 200:
                case 201:
                    return;
                case 409:
                    throw new Exception(message: "Prato duplicado, nome repetido!");
                default:
                    throw new Exception(message: "Error no servidor!");
            }
        }

        public async Task<IEnumerable<Ingredient>> GetIngredientsAsync(string id)
        {
            if (IsConnected)
            {
                var responseMessage = await _client.GetAsync($"{_endPoint}/{id}/ingredients");
                HandleResponseCode((int)responseMessage.StatusCode);

                var json = await responseMessage.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Ingredient>>(json));
            }
            else
                throw new Exception(message: StaticError.NoConnection);
        }

        public async Task<bool> RemoveIngredient(string dishId, string ingrendientId)
        {
            if (string.IsNullOrEmpty(dishId) && string.IsNullOrEmpty(ingrendientId) && !IsConnected)
                return false;

            var response = await _client.DeleteAsync($"{_endPoint}/{dishId}/ingredients/{ingrendientId}");
            HandleResponseCode((int)response.StatusCode);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddIngredient(string dishId, string ingrendientId)
        {
            if (string.IsNullOrEmpty(dishId) && string.IsNullOrEmpty(ingrendientId) && !IsConnected)
                return false;

            var response = await _client.PostAsync($"{_endPoint}/{dishId}/ingredients/{ingrendientId}", null);
            HandleResponseCode((int)response.StatusCode);

            return response.IsSuccessStatusCode;
        }
    }
}
