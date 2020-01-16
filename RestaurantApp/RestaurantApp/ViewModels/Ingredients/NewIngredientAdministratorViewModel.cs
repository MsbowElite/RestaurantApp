using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Ingredients
{
    public class NewIngredientAdministratorViewModel : BaseViewModel<Ingredient>
    {
        public Ingredient Item { get; set; }
        public bool New { get; set; }
        public IDataStore<Ingredient> DataStore => DependencyService.Get<IDataStore<Ingredient>>();
        public NewIngredientAdministratorViewModel(Ingredient item = null)
        {
            if (item != null)
            {
                Title = "Editar Ingrediente";
                Item = item;
            }
            else
            {
                Title = "Novo Ingrediente";
                Item = new Ingredient();
                New = true;
            }
        }

        public async Task<Ingredient> Add()
        {
            Item.CompanyId = new Guid(Preferences.Get("company", ""));
            Item = await DataStore.AddItemAsync(Item);

            if (Item != null && Item.Id != new Guid())
            {
                return Item;
            }

            return null;
        }

        public async Task<Ingredient> Edit()
        {
            Item.CompanyId = new Guid(Preferences.Get("company", ""));
            if(await DataStore.UpdateItemAsync(Item))
            {
                Item = await DataStore.GetItemAsync(Item.Id.ToString());
            }

            if (Item != null && Item.Id != new Guid())
            {
                return Item;
            }

            return null;
        }
    }
}
