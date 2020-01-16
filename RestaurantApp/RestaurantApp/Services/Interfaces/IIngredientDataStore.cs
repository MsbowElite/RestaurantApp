using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services.Interfaces
{
    public interface IIngredientDataStore : IDataStore<Ingredient>
    {
        Task<IEnumerable<Ingredient>> GetItemsExcludeByDishIdAsync(string dishId);
    }
}
