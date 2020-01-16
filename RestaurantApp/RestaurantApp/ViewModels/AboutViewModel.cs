using RestaurantApp.Models;
using System;
using System.Windows.Input;

using Xamarin.Forms;

namespace RestaurantApp.ViewModels
{
    public class AboutViewModel : BaseViewModel<Item>
    {
        public AboutViewModel()
        {
            Title = "About";

            OpenWebCommand = new Command(() => Device.OpenUri(new Uri("https://xamarin.com/platform")));
        }

        public ICommand OpenWebCommand { get; }
    }
}