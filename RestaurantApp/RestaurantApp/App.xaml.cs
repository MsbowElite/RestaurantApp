using System;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using RestaurantApp.Services;
using RestaurantApp.Views;
using RestaurantApp.Views.Administrator;
using Jose;
using Newtonsoft.Json;
using RestaurantApp.Models;
using Microsoft.IdentityModel.JsonWebTokens;
using System.Collections.Generic;
using System.Globalization;

namespace RestaurantApp
{
    public partial class App : Application
    {
        //TODO: Replace with *.azurewebsites.net url after deploying backend to Azure
        //To debug on Android emulators run the web backend against .NET Core not IIS
        //If using other emulators besides stock Google images you may need to adjust the IP address
        //public static string AzureBackendUrl = "http://10.0.2.2:5000";
        public static string AzureBackendUrl = "xxx"; //Remember to set this value /MsbowElite
                                                      //DeviceInfo.Platform == DevicePlatform.Android ? "http://10.0.2.2:5000" : "http://localhost:5000";
        public static bool UseMockDataStore = true;

        public App()
        {
            InitializeComponent();

            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("xxx"); //Remember to set this value /MsbowElite

            if (UseMockDataStore)
            {
                DependencyService.Register<MockDataStore>();
            }
            else
            {
                DependencyService.Register<AzureDataStore>();
            }

            DependencyService.Register<IngredientDataStore>();
            DependencyService.Register<CompanyDataStore>();
            DependencyService.Register<DishDataStore>();
            DependencyService.Register<MenuDataStore>();

            var token = Preferences.Get("token", "");
            if (!string.IsNullOrEmpty(token))
            {
                var payload = new JsonWebToken(token);
                payload.TryGetClaim("exp", out System.Security.Claims.Claim expire);

                var tokenExpiration = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
                tokenExpiration = tokenExpiration.AddSeconds(Convert.ToDouble(expire.Value));

                if (DateTime.UtcNow > tokenExpiration)
                {
                    Preferences.Remove("token");
                    token = null;
                }

                MainPage = new MasterAdministratorPage();
            }
            //else if (string.IsNullOrEmpty(Preferences.Get("useremail", "")) && string.IsNullOrEmpty(Preferences.Get("password", "")))
            if(string.IsNullOrEmpty(token))
            {
                MainPage = new NavigationPage(new LoginPage());
            }
        }

        protected override void OnStart()
        {
            // Handle when your app starts
        }

        protected override void OnSleep()
        {
            // Handle when your app sleeps
        }

        protected override void OnResume()
        {
            // Handle when your app resumes
        }
    }
}
