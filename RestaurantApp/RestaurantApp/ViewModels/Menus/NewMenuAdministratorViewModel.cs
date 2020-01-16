using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Services.Interfaces;
using RestaurantApp.Views.Administrator.Dishes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Menus
{
    public class NewMenuAdministratorViewModel : BaseViewModel<CompanyMenu>
    {
        public CompanyMenu Item { get; set; }
        public ObservableCollection<Dish> Dishes { get; set; }
        public Command LoadItemCommand { get; set; }
        public bool New { get; set; }
        public bool NewInverted 
        {
            get
            {
                return !New;
            }
        }
        public IMenuDataStore DataStore => DependencyService.Get<IMenuDataStore>();
        public NewMenuAdministratorViewModel(CompanyMenu item = null)
        {
            Dishes = new ObservableCollection<Dish>();
            if (item != null)
            {
                Title = "Editar Menu";
                Item = item;
                LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
            }
            else
            {
                Title = "Novo Menu";
                Item = new CompanyMenu();
                New = true;
            }

            MessagingCenter.Subscribe<DishesAdministratorPage, Dish>(this, "SelectDishe", async (obj, dishe) =>
            {
                if(dishe != null)
                await AddDishe(dishe as Dish);
            });
        }

        public async Task AddDishe(Dish dishe)
        {
            try
            {
                if (await DataStore.AddDishe(Item.Id.ToString(), dishe.Id.ToString()))
                {
                    Dishes.Add(dishe);
                }
                else
                {
                    MessagingCenter.Send(this, "NewMenuAlert", "Não foi possível adicionar o prato");
                }
            }catch(Exception ex)
            {
                MessagingCenter.Send(this, "NewMenuAlert", ex.Message);
            }
        }

        public async Task RemoveDish(Dish dishe)
        {
            if (await DataStore.RemoveDishe(Item.Id.ToString(), dishe.Id.ToString()))
            {
                Dishes.Remove(dishe);
            }
            else
            {
                throw new Exception(message: "Não foi possível remover o prato");
            }
        }

        public async Task<CompanyMenu> Add()
        {
            Item.CompanyId = new Guid(Preferences.Get("company", ""));
            Item = await DataStore.AddItemAsync(Item);

            if (Item != null && Item.Id != new Guid())
            {
                return Item;
            }

            return null;
        }

        public async Task<CompanyMenu> Edit()
        {
            Item.CompanyId = new Guid(Preferences.Get("company", ""));
            if(await DataStore.UpdateItemAsync(Item))
            {
                Item = await DataStore.GetItemAsync(Item.Id.ToString());
                if (Item != null && Item.Id != new Guid())
                {
                    return Item;
                }
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
