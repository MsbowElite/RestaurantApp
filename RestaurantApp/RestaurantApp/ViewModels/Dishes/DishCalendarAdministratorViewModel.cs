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
    public class DishCalendarAdministratorViewModel : BaseViewModel<Item>
    {
        public ObservableCollection<Item> Items { get; set; }
        public CalendarEventCollection Appointments { get; set; }
        public DateTime SelectedDate { get; set; }

        public Command LoadItemsCommand { get; set; }
        ICompanyDataStore _dataStore => DependencyService.Get<ICompanyDataStore>();
        public DishCalendarAdministratorViewModel()
        {
            Title = "Calendars";
            Items = new ObservableCollection<Item>();
            Appointments = new CalendarEventCollection();
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
                var dishCalendars = await _dataStore.GetDishCalendarsOwnMonth(companyId.ToString(), (byte)SelectedDate.Month, SelectedDate.Year);

                Appointments.Clear();
                foreach (var dishCalendar in dishCalendars)
                {
                    var appointment = new CalendarInlineEvent();
                    appointment.Subject = "TOP";
                    appointment.Color = Color.GreenYellow;
                    appointment.StartTime = dishCalendar.StartTime;
                    appointment.EndTime = dishCalendar.EndTime;

                    Appointments.Add(appointment);
                }
                this.OnPropertyChanged("Appointments");
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