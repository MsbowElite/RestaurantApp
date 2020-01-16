using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

using RestaurantApp.Models;
using RestaurantApp.Views;
using RestaurantApp.ViewModels;
using RestaurantApp.ViewModels.Dishes;

namespace RestaurantApp.Views.Administrator.Dishes
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DishCalendarAdministratorPage : ContentPage
    {
        DishCalendarAdministratorViewModel viewModel;

        public DishCalendarAdministratorPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DishCalendarAdministratorViewModel();
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewDishCalendarAdministratorPage()));
        }

        void OnSelectionChanged(object sender, Syncfusion.SfCalendar.XForms.SelectionChangedEventArgs e)
        {
            viewModel.SelectedDate = e.DateAdded.FirstOrDefault();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            viewModel.SelectedDate = CalendarDish.SelectedDate.Value;
            viewModel.LoadItemsCommand.Execute(null);
        }

        private async void CalendarDish_InlineItemTapped(object sender, Syncfusion.SfCalendar.XForms.InlineItemTappedEventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new DishCalendarsAdministratorPage(viewModel.SelectedDate)));
        }
    }
}