using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class LoginViewModel
    {
        UserService userService;
        ICompanyDataStore _companyDataStore => DependencyService.Get<ICompanyDataStore>();
        public LoginViewModel()
        {
            userService = new UserService();
            Preferences.Set("token", userService.Authenticate().Token);
            userService.SetupToken();
        }

        public async Task<bool> Login(string userName, string password)
        {
            var user = new UserModel() { Username = userName, Password = password };
            var auth = await userService.Login(user);

            if (auth != null && auth.Id != new Guid())
            {
                Preferences.Set("useremail", auth.Username);
                Preferences.Set("token", auth.Token);
                userService.SetupToken();
                _companyDataStore.SetupToken();
                var companies = await _companyDataStore.GetMyOwnListAsync(true);
                Preferences.Set("company", companies.First().Id.ToString());

                return true;
            }

            return false;
        }
    }
}
