using System;
using System.ComponentModel;
using Xamarin.Forms;
using RestaurantApp.Models;
using RestaurantApp.ViewModels.Dishes;

namespace RestaurantApp.Views.Administrator.Dishes
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DishesAdministratorPage : ContentPage
    {
        DishesAdministratorViewModel viewModel;

        public DishesAdministratorPage(bool select = false)
        {
            InitializeComponent();

            BindingContext = viewModel = new DishesAdministratorViewModel(select);
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ItemsListView.SelectedItem = null;
            var item = args.SelectedItem as Dish;
            if (item == null)
                return;

            if (viewModel.Select)
            {
                MessagingCenter.Send(this, "SelectDish", item);
                await Navigation.PopModalAsync();
            }
            else
            {
                await Navigation.PushAsync(new DisheDetailAdministratorPage(new DisheDetailAdministratorViewModel(item)));
            }
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewDishAdministratorPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}