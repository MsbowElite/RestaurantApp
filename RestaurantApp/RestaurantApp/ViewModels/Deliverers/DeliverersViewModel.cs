using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Views.Administrator.Deliverers;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Deliverers
{
    public class DeliverersAdministratorViewModel : BaseViewModel<Deliverer>
    {
        public ObservableCollection<Deliverer> Items { get; set; }
        public Command LoadItemsCommand { get; set; }
        public IDataStore<Deliverer> DataStore => DependencyService.Get<IDataStore<Deliverer>>();
        public DeliverersAdministratorViewModel()
        {
            Title = "Delivereres";
            Items = new ObservableCollection<Deliverer>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            MessagingCenter.Subscribe<NewDelivererAdministratorPage, Deliverer>(this, "AddItem", async (obj, item) =>
            {
                var newItem = item as Deliverer;
                Items.Add(newItem);
            });

            MessagingCenter.Subscribe<NewDelivererAdministratorPage, Deliverer>(this, "EditItem", async (obj, item) =>
            {
                var newItem = item as Deliverer;
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
