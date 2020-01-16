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
    public partial class IngredientsAdministratorPage : ContentPage
    {
        IngredientsAdministratorViewModel viewModel;

        public IngredientsAdministratorPage(bool select = false, string dishId = null)
        {
            InitializeComponent();

            BindingContext = viewModel = new IngredientsAdministratorViewModel(select, dishId);

            viewModel.LoadItemsCommand.Execute(null);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ItemsListView.SelectedItem = null;

            var item = args.SelectedItem as Ingredient;
            if (item == null)
                return;

            if (viewModel.Select)
            {
                MessagingCenter.Send(this, "SelectIngredient", item);
                await Navigation.PopModalAsync();
            }
            else
            {
                await Navigation.PushAsync(new IngredientDetailAdministratorPage(new IngredientDetailAdministratorViewModel(item)));
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewIngredientAdministratorPage()));
        }
    }
}