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
    public partial class NewDishAdministratorPage : ContentPage
    {
        readonly NewDishAdministratorViewModel viewModel;

        public NewDishAdministratorPage(Dish Dishe = null)
        {
            InitializeComponent();

            BindingContext = viewModel = new NewDishAdministratorViewModel(Dishe);

            if (!viewModel.New)
                viewModel.LoadItemCommand.Execute(null);

            MessagingCenter.Subscribe<NewDishAdministratorViewModel, string>(this, "NewDishAlert", async (obj, message) =>
            {
                await DisplayAlert("Erro", message, "Ok");
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                ItemsListView.SelectedItem = null;
                var item = args.SelectedItem as Ingredient;
                if (item == null)
                    return;

                if (await DisplayAlert("Remover", "Deseja remover este prato?", "Sim", "Não"))
                    await viewModel.RemoveIngredient(ItemsListView.SelectedItem as Ingredient);
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
            }
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

        async void AddIngredient_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new IngredientsAdministratorPage(true, viewModel.Item.Id)));
        }
    }
}