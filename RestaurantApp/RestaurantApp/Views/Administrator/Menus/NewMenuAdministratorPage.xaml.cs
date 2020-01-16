using RestaurantApp.Models;
using RestaurantApp.ViewModels.Menus;
using RestaurantApp.Views.Administrator.Dishes;
using System;
using System.ComponentModel;
using Xamarin.Forms;

namespace RestaurantApp.Views.Administrator.Menus
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class NewMenuAdministratorPage : ContentPage
    {
        readonly NewMenuAdministratorViewModel viewModel;

        public NewMenuAdministratorPage(CompanyMenu Menu = null)
        {
            InitializeComponent();

            BindingContext = viewModel = new NewMenuAdministratorViewModel(Menu);

            if (!viewModel.New)
                viewModel.LoadItemCommand.Execute(null);

            MessagingCenter.Subscribe<NewMenuAdministratorViewModel, string>(this, "NewMenuAlert", async (obj, message) =>
            {
                await DisplayAlert("Erro", message, "Ok");
            });
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            try
            {
                ItemsListView.SelectedItem = null;
                var item = args.SelectedItem as Dish;
                if (item == null)
                    return;

                if (await DisplayAlert("Remover", "Deseja remover este prato?", "Sim", "Não"))
                    await viewModel.RemoveDish(ItemsListView.SelectedItem as Dish);
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
                var menu = await viewModel.Edit();
                    if (menu != null)
                    {
                        MessagingCenter.Send(this, "EditItem", menu);
                        await Navigation.PopModalAsync();
                    }
                    else
                    {
                        await DisplayAlert("Erro", "O item não foi atualizado, requisição sem sucesso.", "Ok");
                    }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Erro", ex.Message, "Ok");
            }
        }

        async void Cancel_Clicked(object sender, EventArgs e)
        {
            await Navigation.PopModalAsync();
        }

        async void AddDish_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new DishesAdministratorPage(true)));
        }
    }
}