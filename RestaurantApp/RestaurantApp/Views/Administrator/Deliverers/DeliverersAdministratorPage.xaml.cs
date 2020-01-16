using System;
using System.ComponentModel;
using Xamarin.Forms;
using RestaurantApp.Models;
using RestaurantApp.ViewModels.Deliverers;

namespace RestaurantApp.Views.Administrator.Deliverers
{
    // Learn more about making custom code visible in the Xamarin.Forms previewer
    // by visiting https://aka.ms/xamarinforms-previewer
    [DesignTimeVisible(false)]
    public partial class DeliverersAdministratorPage : ContentPage
    {
        DeliverersAdministratorViewModel viewModel;

        public DeliverersAdministratorPage()
        {
            InitializeComponent();

            BindingContext = viewModel = new DeliverersAdministratorViewModel();
        }

        async void OnItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var item = args.SelectedItem as Deliverer;
            if (item == null)
                return;

            await Navigation.PushAsync(new DelivererDetailAdministratorPage(new DelivererDetailAdministratorViewModel(item)));

            // Manually deselect item.
            ItemsListView.SelectedItem = null;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewDelivererAdministratorPage()));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Items.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }
    }
}