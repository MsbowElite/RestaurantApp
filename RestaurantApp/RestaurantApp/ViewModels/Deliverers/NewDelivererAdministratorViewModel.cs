using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Deliverers
{
    public class NewDelivererAdministratorViewModel : BaseViewModel<Deliverer>
    {
        public Deliverer Item { get; set; }
        public bool New { get; set; }
        public IDataStore<Deliverer> DataStore => DependencyService.Get<IDataStore<Deliverer>>();
        public NewDelivererAdministratorViewModel(Deliverer item = null)
        {
            if (item != null)
            {
                Title = "Editar Deliverere";
                Item = item;
            }
            else
            {
                Title = "Novo Deliverere";
                Item = new Deliverer();
            }
        }

        public async Task<Deliverer> Add()
        {
            Item.CompanyId = new Guid(Preferences.Get("company", ""));
            Item = await DataStore.AddItemAsync(Item);

            if (Item != null && Item.Id != new Guid())
            {
                return Item;
            }

            return null;
        }

        public async Task<Deliverer> Edit()
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
