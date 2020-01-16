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
    public partial class DelivererDetailAdministratorPage : ContentPage
    {
        DelivererDetailAdministratorViewModel viewModel;

        public DelivererDetailAdministratorPage(DelivererDetailAdministratorViewModel viewModel)
        {
            InitializeComponent();

            BindingContext = this.viewModel = viewModel;
        }

        public DelivererDetailAdministratorPage()
        {
            InitializeComponent();

            var item = new Deliverer
            {
                Name = "Item 1"
            };

            viewModel = new DelivererDetailAdministratorViewModel(item);
            BindingContext = viewModel;
        }

        async void AddItem_Clicked(object sender, EventArgs e)
        {
            await Navigation.PushModalAsync(new NavigationPage(new NewDelivererAdministratorPage(viewModel.Item)));
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            viewModel.LoadItemCommand.Execute(null);
        }
    }
}