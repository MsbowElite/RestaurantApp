using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Services.Interfaces;
using RestaurantApp.Views.Administrator.Menus;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Menus
{
    public class MenuDetailAdministratorViewModel : BaseViewModel<CompanyMenu>
    {
        public CompanyMenu Item { get; set; }
        public ObservableCollection<Dish> Dishes { get; set; }
        public Command LoadItemCommand { get; set; }
        public IMenuDataStore DataStore => DependencyService.Get<IMenuDataStore>();
        public MenuDetailAdministratorViewModel(CompanyMenu item = null)
        {
            Dishes = new ObservableCollection<Dish>();

            Title = item?.Name;
            Item = item;

            LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());

            MessagingCenter.Subscribe<NewMenuAdministratorPage, CompanyMenu>(this, "EditItem", async (obj, menu) =>
            {
                Item = menu as CompanyMenu;
                Title = Item.Name;
            });
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

                var dishes = await DataStore.GetDishesAsync(Item.Id.ToString());
                Dishes.Clear();
                foreach (var dishe in dishes)
                {
                    Dishes.Add(dishe);
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
