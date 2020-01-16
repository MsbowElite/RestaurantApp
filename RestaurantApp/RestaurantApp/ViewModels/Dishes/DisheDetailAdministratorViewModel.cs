using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Dishes
{
    public class DisheDetailAdministratorViewModel : BaseViewModel<Dish>
    {
        public Dish Item { get; set; }
        public Command LoadItemCommand { get; set; }
        public ObservableCollection<Ingredient> Ingredients { get; set; }
        public IDishDataStore DataStore => DependencyService.Get<IDishDataStore>();
        public DisheDetailAdministratorViewModel(Dish item = null)
        {
            Ingredients = new ObservableCollection<Ingredient>();

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
