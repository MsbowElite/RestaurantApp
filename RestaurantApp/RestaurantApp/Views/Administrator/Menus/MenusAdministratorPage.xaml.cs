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
    public partial class MenusAdministratorPage : ContentPage
    {
        MenusAdministratorViewModel viewModel;

        public MenusAdministratorPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new MenusAdministratorViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as CompanyMenu;
            if (item == null)
                return;

            await Navigation.PushAsync(new MenuDetailAdministratorPage(new MenuDetailAdministratorViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewMenuAdministratorPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}