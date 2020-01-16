﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Administrator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MasterAdministratorPage : MasterDetailPage
    {
        public MasterAdministratorPage()
        {
            NavigationPage.SetHasNavigationBar(this, true);
            InitializeComponent();
        }

        private void TapHome_OnTapped(object sender, EventArgs e)
        {
            //Detail = new NavigationPage(new HomePage());
            IsPresented = false;
        }

        private void TapChangePassword_OnTapped(object sender, EventArgs e)
        {
            //Detail = new NavigationPage(new ChangePasswordPage());
            IsPresented = false;
        }

        private void TapBecomeInstructor_OnTapped(object sender, EventArgs e)
        {
            //Detail = new NavigationPage(new BecomeInstructorPage());
            IsPresented = false;
        }

        private void TapFaq_OnTapped(object sender, EventArgs e)
        {
            //Detail = new NavigationPage(new FaqPage());
            IsPresented = false;
        }

        private void TapLogout_OnTapped(object sender, EventArgs e)
        {
            Preferences.Set("useremail", string.Empty);
            Preferences.Set("token", string.Empty);
            Preferences.Set("password", string.Empty);
            Preferences.Set("accesstoken", string.Empty);
            Preferences.Set("company", string.Empty);

            Application.Current.MainPage = new NavigationPage(new LoginPage());
        }
    }
}