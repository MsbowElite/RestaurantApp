using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services.Interfaces
{
    public interface IDishDataStore : IDataStore<Dish>
    {
        Task<IEnumerable<Ingredient>> GetIngredientsAsync(string id);
        Task<bool> RemoveIngredient(string dishId, string ingrendientId);
        Task<bool> AddIngredient(string dishId, string ingrendientId);
    }
}
