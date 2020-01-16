using RestaurantApp.Helpers;
using RestaurantApp.Models;
using RestaurantApp.Services.Interfaces;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;

namespace RestaurantApp.Services
{
    public class MenuDataStore : DataStore<CompanyMenu>, IMenuDataStore
    {

        public MenuDataStore() : base("api/CompanyMenus")
        {
            _client = new HttpClient();
            _client.BaseAddress = new Uri($"{App.AzureBackendUrl}");
            SetupToken();

            MessageStatus409 = "Item duplicado, nome repetido!";

            _items = new List<CompanyMenu>();
        }

        public async Task<IEnumerable<CompanyMenu>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                var responseMessage = await _client.GetAsync(_endPoint);
                HandleResponseCode((int)responseMessage.StatusCode);

                var json = await responseMessage.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<CompanyMenu>>(json));
            }
            return _items;
        }

        public async Task<CompanyMenu> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                var json = await _client.GetStringAsync($"{_endPoint}/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<CompanyMenu>(json));
            }

            return null;
        }

        public async Task<CompanyMenu> AddItemAsync(CompanyMenu item)
        {
            if (item == null)
                throw new Exception(message: "Menu faltando valores");
            if (!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            var serializedItem = JsonConvert.SerializeObject(item);
            var response = await _client.PostAsync($"{_endPoint}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            HandleResponseCode((int)response.StatusCode);
            var json = await response.Content.ReadAsStringAsync();

            return await Task.Run(() => JsonConvert.DeserializeObject<CompanyMenu>(json));
        }

        public async Task<bool> UpdateItemAsync(CompanyMenu item)
        {
            if (item == null || item.Id == null || !IsConnected)
                throw new Exception(message: "Menu faltando valores");
            if (!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            var serializedItem = JsonConvert.SerializeObject(item);
            var response = await _client.PutAsync($"{_endPoint}/{item.Id.ToString()}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));
            HandleResponseCode((int)response.StatusCode);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            var response = await _client.DeleteAsync($"{_endPoint}/{id}");
            HandleResponseCode((int)response.StatusCode);

            return response.IsSuccessStatusCode;
        }

        #region Dishes
        public async Task<IEnumerable<Dish>> GetDishesAsync(string id)
        {
            if (IsConnected)
            {
                var responseMessage = await _client.GetAsync($"{_endPoint}/{id}/dishes");
                HandleResponseCode((int)responseMessage.StatusCode);

                var json = await responseMessage.Content.ReadAsStringAsync();
                return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Dish>>(json));
            }
            else
                throw new Exception(message: StaticError.NoConnection);
        }

        public async Task<bool> RemoveDishe(string menuId, string dishId)
        {
            if (string.IsNullOrEmpty(menuId) && string.IsNullOrEmpty(dishId) && !IsConnected)
                return false;

            var response = await _client.DeleteAsync($"{_endPoint}/{menuId}/dishe/{dishId}");
            HandleResponseCode((int)response.StatusCode);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> AddDishe(string menuId, string dishId)
        {
            if (string.IsNullOrEmpty(menuId) && string.IsNullOrEmpty(dishId) && !IsConnected)
                return false;

            var response = await _client.PostAsync($"{_endPoint}/{menuId}/dishe/{dishId}", null);
            HandleResponseCode((int)response.StatusCode);

            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
