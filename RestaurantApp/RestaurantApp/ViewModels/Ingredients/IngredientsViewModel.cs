using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Services.Interfaces;
using RestaurantApp.Views.Administrator.Ingredients;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Ingredients
{
    public class IngredientsAdministratorViewModel : BaseViewModel<Ingredient>
    {
        public ObservableCollection<Ingredient> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SearchItemsCommand { get; set; }
        public bool Select { get; set; }
        string _filter { get; set; }
        string _dishId { get; set; }
        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                Task.Run(() => ExecuteSearchItemsCommand());
            }
        }
        public IIngredientDataStore DataStore => DependencyService.Get<IIngredientDataStore>();
        public IngredientsAdministratorViewModel(bool select = false, string dishId = null)
        {
            Title = "Ingredientes";
            Select = select;
            _dishId = dishId;
            Items = new ObservableCollection<Ingredient>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SearchItemsCommand = new Command(async () => await ExecuteSearchItemsCommand());

            MessagingCenter.Subscribe<NewIngredientAdministratorPage, Ingredient>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Ingredient;
                Items.Insert(0, newItem);
            });

            MessagingCenter.Subscribe<NewIngredientAdministratorPage, Ingredient>(this, "EditItem", async (obj, item) =>
            {
                var newItem = item as Ingredient;
                var oldItem = Items.First(f => f.Id == newItem.Id);
                Items.Remove(oldItem);
                Items.Insert(0, newItem);
            });
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                if (Select && !string.IsNullOrEmpty(_dishId))
                {
                    Items = new ObservableCollection<Ingredient>(await DataStore.GetItemsExcludeByDishIdAsync(_dishId));
                }
                else
                {
                    Items = new ObservableCollection<Ingredient>(await DataStore.GetItemsAsync(true));
                }
                OnPropertyChanged("Items");
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
        async Task ExecuteSearchItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                Items = new ObservableCollection<Ingredient>((await DataStore.GetItemsAsync(false))
                    .Where(w => w.Name.ToLower().Contains(Filter.ToLower()))
                    .OrderBy(o => o.Name));
                OnPropertyChanged("Items");
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
