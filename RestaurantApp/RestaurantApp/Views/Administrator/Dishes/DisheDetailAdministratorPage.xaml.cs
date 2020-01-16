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
    public partial class DisheDetailAdministratorPage : ContentPage
    {
        DisheDetailAdministratorViewModel viewModel;

        public DisheDetailAdministratorPage(DisheDetailAdministratorViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public DisheDetailAdministratorPage()
        {
            InitializeComponent();

            var item = new Dish
            {
                Name = "Item 1",
                Description = "This is an item description."
            };

            viewModel = new DisheDetailAdministratorViewModel(item);
            BindingContext = viewModel;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewDishAdministratorPage(viewModel.Item)));
        }

        void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            ItemsListView.SelectedItem = null;
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemCommand.Execute(null);
        }
    }
}