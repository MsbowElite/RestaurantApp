using RestaurantApp.Models;
using RestaurantApp.Services.Interfaces;
using RestaurantApp.Views.Administrator.Ingredients;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Dishes
{
    public class NewDishAdministratorViewModel : BaseViewModel<Dish>
    {
        public Dish Item { get; set; }
        public bool New { get; set; }
        public bool NewInverted
        {
            get
            {
                return !New;
            }
        }
        public Command LoadItemCommand { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public IDishDataStore DataStore => DependencyService.Get<IDishDataStore>();
        public NewDishAdministratorViewModel(Dish item = null)
        {
            Ingredients = new ObservableCollection<Ingredient>();
            if (item != null)
            {
                Title = "Editar Prato";
                Item = item;
                LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
            }
            else
            {
                Title = "Novo Prato";
                Item = new Dish();
                New = true;
            }

            MessagingCenter.Subscribe<IngredientsAdministratorPage, Ingredient>(this, "SelectIngredient", async (obj, ingredient) =>
            {
                if (ingredient != null)
                    await AddIngredient(ingredient as Ingredient);
            });
        }

        public async Task<Dish> Add()
        {
            Item.CompanyId = (Preferences.Get("company", ""));
            Item = await DataStore.AddItemAsync(Item);

            if (Item != null && !String.IsNullOrEmpty(Item.Id))
            {
                return Item;
            }

            return null;
        }

        public async Task AddIngredient(Ingredient ingredient)
        {
            try
            {
                if (await DataStore.AddIngredient(Item.Id.ToString(), ingredient.Id.ToString()))
                {
                    Ingredients.Add(ingredient);
                }
                else
                {
                    MessagingCenter.Send(this, "NewDishAlert", "Não foi possível adicionar o prato");
                }
            }
            catch (Exception ex)
            {
                MessagingCenter.Send(this, "NewDishAlert", ex.Message);
            }
        }

        public async Task RemoveIngredient(Ingredient ingredient)
        {
            if (await DataStore.RemoveIngredient(Item.Id.ToString(), ingredient.Id.ToString()))
            {
                Ingredients.Remove(ingredient);
            }
            else
            {
                throw new Exception(message: "Não foi possível remover o prato");
            }
        }

        public async Task<Dish> Edit()
        {
            Item.CompanyId = Preferences.Get("company", "");
            if(await DataStore.UpdateItemAsync(Item))
            {
                Item = await DataStore.GetItemAsync(Item.Id.ToString());
            }

            if (Item != null && string.IsNullOrEmpty(Item.Id))
            {
                return Item;
            }

            return null;
        }

        async Task ExecuteLoadItemCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Item = await DataStore.GetItemAsync(Item.Id.ToString());
                OnPropertyChanged("Item");

                var ingredients = await DataStore.GetIngredientsAsync(Item.Id.ToString());
                Ingredients.Clear();
                foreach (var ingredient in ingredients)
                {
                    Ingredients.Add(ingredient);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
