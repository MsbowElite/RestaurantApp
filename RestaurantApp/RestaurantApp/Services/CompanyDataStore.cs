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

namespace RestaurantApp.Services
{
    public class CompanyDataStore : DataStore<Company>, ICompanyDataStore
    {
        public CompanyDataStore() : base("api/Companies")
        {
            _client = new HttpClient();
            _client.Timeout = new TimeSpan(60000000);
            _client.BaseAddress = new Uri($"{App.AzureBackendUrl}");
            SetupToken();

            _items = new List<Company>();
        }

        public async Task<IEnumerable<Company>> GetItemsAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                var responseMessage = await _client.GetAsync(_endPoint);

                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                    case 201:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Company>>(json));
                    default:
                        throw new Exception(message: "Error Default");
                }
            }

            return _items;
        }

        public async Task<IEnumerable<Company>> GetMyOwnListAsync(bool forceRefresh = false)
        {
            if (forceRefresh && IsConnected)
            {
                var responseMessage = await _client.GetAsync($"{_endPoint}/GetMyOwnList");

                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                    case 201:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<Company>>(json));
                    default:
                        throw new Exception(message: "Error Default");
                }
            }

            return _items;
        }

        public async Task<Company> GetItemAsync(string id)
        {
            if (id != null && IsConnected)
            {
                var json = await _client.GetStringAsync($"{_endPoint}/{id}");
                return await Task.Run(() => JsonConvert.DeserializeObject<Company>(json));
            }

            return null;
        }

        public async Task<Company> AddItemAsync(Company item)
        {
            if (item == null)
                throw new Exception(message: "Companye faltando valores");
            if (!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            var serializedItem = JsonConvert.SerializeObject(item);

            var response = await _client.PostAsync($"{_endPoint}", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            switch ((int)response.StatusCode)
            {
                case 200:
                case 201:
                    var json = await response.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<Company>(json));
                default:
                    throw new Exception(message: "Error Default");
            }
        }

        public async Task<bool> UpdateItemAsync(Company item)
        {
            if (item == null || item.Id == null || !IsConnected)
                return false;

            var serializedItem = JsonConvert.SerializeObject(item);
            var buffer = Encoding.UTF8.GetBytes(serializedItem);
            var byteContent = new ByteArrayContent(buffer);

            var response = await _client.PutAsync(new Uri($"{_endPoint}/{item.Id}"), byteContent);

            return response.IsSuccessStatusCode;
        }

        public async Task<bool> DeleteItemAsync(string id)
        {
            if (string.IsNullOrEmpty(id) && !IsConnected)
                return false;

            var response = await _client.DeleteAsync($"{_endPoint}/{id}");

            return response.IsSuccessStatusCode;
        }

        #region DishCalendar
        public async Task<IEnumerable<DishCalendar>> GetDishCalendarsOwnMonth(string id, byte month, int year)
        {
            if (IsConnected)
            {
                var responseMessage = await _client.GetAsync($"{_endPoint}/{id}/Calendars/month/{month}/year/{year}/Dishes");

                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                    case 201:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<DishCalendar>>(json));
                    default:
                        throw new Exception(message: "Error Default");
                }
            }
            throw new Exception(message: "Error Default");
        }
        public async Task<IEnumerable<DishCalendarDate>> GetDishCalendarsOwnDate(string id, long dateTicks)
        {
            if (IsConnected)
            {
                var responseMessage = await _client.GetAsync($"{_endPoint}/{id}/Calendars/date/{dateTicks}/Dishes");

                switch ((int)responseMessage.StatusCode)
                {
                    case 200:
                    case 201:
                        var json = await responseMessage.Content.ReadAsStringAsync();
                        return await Task.Run(() => JsonConvert.DeserializeObject<IEnumerable<DishCalendarDate>>(json));
                    default:
                        throw new Exception(message: "Error Default");
                }
            }
            throw new Exception(message: "Error Default");
        }
        public async Task<DishCalendar> AddDishCalendarsOwn(DishCalendar item)
        {
            if (item == null)
                throw new Exception(message: "Faltando valores no agendamento");
            if (!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            if (string.IsNullOrEmpty(item.DishId))
                throw new Exception(message: "Falta selecionar o prato!");

            var serializedItem = JsonConvert.SerializeObject(item);
            var response = await _client.PostAsync($"{_endPoint}/{item.CompanyId}/Calendars/Dishes", new StringContent(serializedItem, Encoding.UTF8, "application/json"));

            switch ((int)response.StatusCode)
            {
                case 200:
                case 201:
                    var json = await response.Content.ReadAsStringAsync();
                    return await Task.Run(() => JsonConvert.DeserializeObject<DishCalendar>(json));
                default:
                    throw new Exception(message: "Error Default");
            }
        }

        public async Task<bool> RemoveDishCalendarsOwn(DishCalendar item, long dateTicks)
        {
            if (item == null)
                throw new Exception(message: "Faltando valores no agendamento");
            if (!IsConnected)
                throw new Exception(message: StaticError.NoConnection);

            if (string.IsNullOrEmpty(item.DishId))
                throw new Exception(message: "Falta selecionar o prato!");

            var serializedItem = JsonConvert.SerializeObject(item);
            var response = await _client.DeleteAsync($"{_endPoint}/{item.CompanyId}/Calendars/date/{dateTicks}/Dishes/{item.DishId}");

            HandleResponseCode((int)response.StatusCode);
            return response.IsSuccessStatusCode;
        }
        #endregion
    }
}
