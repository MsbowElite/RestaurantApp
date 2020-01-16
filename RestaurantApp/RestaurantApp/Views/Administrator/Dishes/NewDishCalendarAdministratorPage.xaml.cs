using RestaurantApp.Models;
using RestaurantApp.ViewModels.Dishes;
using RestaurantApp.Views.Administrator.Ingredients;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace RestaurantApp.Views.Administrator.Dishes
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewDishCalendarAdministratorPage : ContentPage
    {
        readonly NewDishCalendarAdministratorViewModel viewModel;

        public NewDishCalendarAdministratorPage(DateTime? date = null)
        {
            InitializeComponent();

            BindingContext = viewModel = new NewDishCalendarAdministratorViewModel(date);

            MessagingCenter.Subscribe<NewDishCalendarAdministratorViewModel, string>(this, "NewDishCalendarAlert", async (obj, message) =>
            {
                await DisplayAlert("Erro", message, "Ok");
            });
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            try
            {
                MessagingCenter.Send(this, "AddItem", await viewModel.Add());
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Alright");
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        private async void SelectDish_OnTapped(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new DishesAdministratorPage(true)));
        }
    }
}