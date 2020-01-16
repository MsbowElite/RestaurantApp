using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Deliverers
{
    public class DelivererDetailAdministratorViewModel : BaseViewModel<Deliverer>
    {
        public Deliverer Item { get; set; }
        public Command LoadItemCommand { get; set; }
        public IDataStore<Deliverer> DataStore => DependencyService.Get<IDataStore<Deliverer>>();
        public DelivererDetailAdministratorViewModel(Deliverer item = null)
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
