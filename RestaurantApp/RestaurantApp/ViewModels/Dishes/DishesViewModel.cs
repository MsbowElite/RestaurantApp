using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Views.Administrator.Dishes;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Dishes
{
    public class DishesAdministratorViewModel : BaseViewModel<Dish>
    {
        public ObservableCollection<Dish> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public Command SearchItemsCommand { get; set; }
        public IDataStore<Dish> DataStore => DependencyService.Get<IDataStore<Dish>>();
        public bool Select { get; set; }
        string _filter { get; set; }
        public string Filter
        {
            get => _filter;
            set
            {
                _filter = value;
                Task.Run(() => ExecuteSearchItemsCommand());
            }
        }

        public DishesAdministratorViewModel(bool select = false)
        {
            Title = "Pratos";
            Items = new ObservableCollection<Dish>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            SearchItemsCommand = new Command(async () => await ExecuteSearchItemsCommand());
            Select = select;

            if (!Select)
            {
                MessagingCenter.Subscribe<NewDishAdministratorPage, Dish>(this, "AddItem", async (obj, item) =>
                {
                    var newItem = item as Dish;
                    if(newItem != null)
                    Items.Insert(0, newItem);
                });

                MessagingCenter.Subscribe<NewDishAdministratorPage, Dish>(this, "EditItem", async (obj, item) =>
                {
                    var newItem = item as Dish;
                    var oldItem = Items.First(f => f.Id == newItem.Id);
                    Items.Remove(oldItem);
                    Items.Insert(0, newItem);
                });
            }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                var items = await DataStore.GetItemsAsync(true);
                foreach (var item in items)
                {
                    Items.Add(item);
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

        async Task ExecuteSearchItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Items.Clear();
                Items = new ObservableCollection<Dish>((await DataStore.GetItemsAsync(false))
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
