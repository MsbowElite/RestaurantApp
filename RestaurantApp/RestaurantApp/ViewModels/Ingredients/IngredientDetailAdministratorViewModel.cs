using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Ingredients
{
    public class IngredientDetailAdministratorViewModel : BaseViewModel<Ingredient>
    {
        public Ingredient Item { get; set; }
        public Command LoadItemCommand { get; set; }
        public IDataStore<Ingredient> DataStore => DependencyService.Get<IDataStore<Ingredient>>();
        public IngredientDetailAdministratorViewModel(Ingredient item = null)
        {
            Title = item?.Name;
            Item = item;

            LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
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
