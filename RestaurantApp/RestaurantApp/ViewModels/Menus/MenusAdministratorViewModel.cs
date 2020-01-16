using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Views.Administrator.Menus;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Menus
{
    public class MenusAdministratorViewModel : BaseViewModel<CompanyMenu>
    {
        public ObservableCollection<CompanyMenu> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public IDataStore<CompanyMenu> DataStore => DependencyService.Get<IDataStore<CompanyMenu>>();
        public MenusAdministratorViewModel()
        {
            Title = "Menus";
            Items = new ObservableCollection<CompanyMenu>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewMenuAdministratorPage, CompanyMenu>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as CompanyMenu;
                Items.Add(newItem);
            });

            MessagingCenter.Subscribe<NewMenuAdministratorPage, CompanyMenu>(this, "EditItem", async (obj, item) =>
            {
                var newItem = item as CompanyMenu;
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
    }
}
