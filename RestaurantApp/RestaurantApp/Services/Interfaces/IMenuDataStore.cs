using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services.Interfaces
{
    public interface IMenuDataStore : IDataStore<CompanyMenu>
    {
        Task<IEnumerable<Dish>> GetDishesAsync(string id);
        Task<bool> RemoveDishe(string menuId, string dishId);
        Task<bool> AddDishe(string menuId, string dishId);
    }
}
