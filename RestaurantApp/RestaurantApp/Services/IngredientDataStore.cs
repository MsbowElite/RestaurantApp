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
    public class IngredientDataStore : DataStore<Ingredient>, IIngredientDataStore
    {
        public IngredientDataStore() : base("api/Ingredients")
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.AzureBackendUrl}");
            SetupToken();

            _items = new List<Ingredient>();
        }

        public async Task<IEnumerable<Ingredient>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                var responseMessage = await _client.GetAsync(_endPoint);
                HandleResponseCode((int)responseMessage.StatusCode);

                var json = await responseMessage.Content.ReadAsStringAsync();
                return _items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Ingredient>>(json));
            }
            return _items;
        }

        public async Task<Ingredient> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                var json = await _client.GetStringAsync($"{_endPoint}/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Ingredient>(json));
            }

            return null;
        }

        public async Task<Ingredient> AddItemAsync(Ingredient item)
        {
            if (item == null)
                throw new Exception(message: "Ingrediente faltando valores");
            if(!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            var serializedItem = JsonConvert.SerializeObject(item);
            var response = await _client.PostAsync($"{_endPoint}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            HandleResponseCode((int)response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<Ingredient>(json));
        }

        public async Task<bool> UpdateItemAsync(Ingredient item)
        {
            if (item == null || item.Id == null || !IsConnected)
                throw new Exception(message: "Ingrediente faltando valores");
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



        public async Task<IEnumerable<Ingredient>> GetItemsExcludeByDishIdAsync(string dishId)
        {
            if (IsConnected)
            {
                var responseMessage = await _client.GetAsync($"{_endPoint}/Dish/{dishId}");
                HandleResponseCode((int)responseMessage.StatusCode);

                var json = await responseMessage.Content.ReadAsStringAsync();
                return _items = await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Ingredient>>(json));
            }
            return _items;
        }
    }
}
