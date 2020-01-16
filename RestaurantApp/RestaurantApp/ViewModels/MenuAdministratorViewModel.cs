using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class MenuAdministratorViewModel : INotifyPropertyChanged
    {
        public IMenuAdministrator<Models.MainMenu> DataStore => new MenuAdministratorMock();
        ICompanyDataStore _companyDataStore => DependencyService.Get<ICompanyDataStore>();
        bool isBusy = false;

        public ObservableCollection<Models.MainMenu> Menus { get; set; }
        public Command LoadItemsCommand { get; set; }

        public MenuAdministratorViewModel()
        {
            Menus = new ObservableCollection<Models.MainMenu>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            //Task.Run(() => CheckCompany());
        }

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetProperty(ref isBusy, value); }
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                Menus.Clear();
                var menus = await DataStore.GetItemsAsync(true);
                foreach (var menu in menus)
                {
                    Menus.Add(menu);
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

        protected bool SetProperty<T>(ref T backingStore, T value,
        [CallerMemberName]string propertyName = "",
        Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        #region INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion
    }
}
