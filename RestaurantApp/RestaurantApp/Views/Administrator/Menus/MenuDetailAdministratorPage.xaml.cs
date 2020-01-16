using System;
using System.ComponentModel;
using Xamarin.Forms;
using RestaurantApp.Models;
using RestaurantApp.ViewModels.Menus;

namespace RestaurantApp.Views.Administrator.Menus
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class MenuDetailAdministratorPage : ContentPage
    {
        MenuDetailAdministratorViewModel viewModel;

        public MenuDetailAdministratorPage(MenuDetailAdministratorViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public MenuDetailAdministratorPage()
        {
            InitializeComponent();

            var item = new CompanyMenu
            {
                Name = "Item 1"
            };

            viewModel = new MenuDetailAdministratorViewModel(item);
            BindingContext = viewModel;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewMenuAdministratorPage(viewModel.Item)));
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