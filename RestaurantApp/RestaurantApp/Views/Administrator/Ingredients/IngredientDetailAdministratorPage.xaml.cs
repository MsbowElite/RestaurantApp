using System;
using System.ComponentModel;
using Xamarin.Forms;
using RestaurantApp.Models;
using RestaurantApp.ViewModels.Ingredients;

namespace RestaurantApp.Views.Administrator.Ingredients
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class IngredientDetailAdministratorPage : ContentPage
    {
        IngredientDetailAdministratorViewModel viewModel;

        public IngredientDetailAdministratorPage(IngredientDetailAdministratorViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public IngredientDetailAdministratorPage()
        {
            InitializeComponent();

            var item = new Ingredient
            {
                Name = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new IngredientDetailAdministratorViewModel(item);
            BindingContext = viewModel;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewIngredientAdministratorPage(viewModel.Item)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemCommand.Execute(null);
        }
    }
}