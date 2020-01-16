using RestaurantApp.Models;
using RestaurantApp.ViewModels.Ingredients;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace RestaurantApp.Views.Administrator.Ingredients
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewIngredientAdministratorPage : ContentPage
    {
        readonly NewIngredientAdministratorViewModel viewModel;

        public NewIngredientAdministratorPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new NewIngredientAdministratorViewModel();
        }

        public NewIngredientAdministratorPage(Ingredient ingredient)
        {
            InitializeComponent();

            BindingContext = viewModel = new NewIngredientAdministratorViewModel(ingredient);
        }

        async void Save_Clicked(object sender, EventArgs e)
        {
            if(viewModel.New)
            try
            {
                MessagingCenter.Send(this, "AddItem", await viewModel.Add());
                await Navigation.PopModalAsync();
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Alright");
            }
            else
            try
            {
                MessagingCenter.Send(this, "EditItem", await viewModel.Edit());
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
    }
}