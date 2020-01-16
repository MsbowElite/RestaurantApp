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
    public partial class DishCalendarsAdministratorPage : ContentPage
    {
        DishCalendarsAdministratorViewModel viewModel;

        public DishCalendarsAdministratorPage(DateTime selectedDate)
        {
            InitializeComponent();
            BindingContext = viewModel = new DishCalendarsAdministratorViewModel(selectedDate);
            viewModel.LoadItemsCommand.Execute(null);
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewDishCalendarAdministratorPage()));
        }

        private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                ItemsListView.SelectedItem = null;
                var item = args.SelectedItem as DishCalendarDate;
                if (item == null)
                    return;

                if (await DisplayAlert("Remover", "Deseja remover este agendamento?", "Sim", "Não"))
                    await viewModel.RemoveDishCalendar(item);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
            }

        }
    }
}