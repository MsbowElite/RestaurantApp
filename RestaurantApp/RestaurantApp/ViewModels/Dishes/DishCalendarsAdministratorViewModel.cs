using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Forms;
using RestaurantApp.Models;
using RestaurantApp.Views;
using RestaurantApp.Services;
using Syncfusion.SfCalendar.XForms;
using Xamarin.Essentials;

namespace RestaurantApp.ViewModels.Dishes
{
    public class DishCalendarsAdministratorViewModel : BaseViewModel<DishCalendarDate>
    {
        public ObservableCollection<DishCalendarDate> Items { get; set; }
        public DateTime SelectedDate { get; set; }
        public Command LoadItemsCommand { get; set; }
        ICompanyDataStore _dataStore => DependencyService.Get<ICompanyDataStore>();
        public DishCalendarsAdministratorViewModel(DateTime selectedDate)
        {
            SelectedDate = selectedDate;
            Title = "Calendars";
            Items = new ObservableCollection<DishCalendarDate>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
        }

        async Task ExecuteLoadItemsCommand()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var companyId = new Guid(Preferences.Get("company", ""));
                var dishCalendars = await _dataStore.GetDishCalendarsOwnDate(companyId.ToString(), SelectedDate.Ticks);

                Items.Clear();
                foreach (var dishCalendar in dishCalendars)
                {
                    Items.Add(dishCalendar);
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

        public async Task RemoveDishCalendar(DishCalendarDate dishCalendarDate)
        {
            DishCalendar dishCalendar = new DishCalendar()
            {
                CompanyId = dishCalendarDate.Dish.CompanyId,
                DishId = dishCalendarDate.Dish.Id
            };
            if (await _dataStore.RemoveDishCalendarsOwn(dishCalendar, dishCalendarDate.StartTime.Date.Ticks))
            {
                Items.Remove(dishCalendarDate);
            }
            else
            {
                throw new Exception(message: "Não foi possível remover o agendamento do prato");
            }
        }
    }
}
