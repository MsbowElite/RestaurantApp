using RestaurantApp.ViewModels;
using RestaurantApp.Views.Administrator;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoginPage : ContentPage
    {
        LoginViewModel viewModel;
        public LoginPage()
        {
            viewModel = new LoginViewModel();
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
        }

        private void TapSignUp_Tapped(object sender, EventArgs e)
        {

        }

        private void TapForgotPassword_Tapped(object sender, EventArgs e)
        {

        }

        private async void BtnLogin_Clicked(object sender, EventArgs e)
        {
            try
            {
                if (await viewModel.Login(EntryEmail.Text, EntryPassword.Text))
                {
                    Application.Current.MainPage = new MasterAdministratorPage();
                }
                else
                {
                    await DisplayAlert("Error", "Something wrong", "Alright");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("Error", ex.Message, "Alright");
            }
        }
    }
}