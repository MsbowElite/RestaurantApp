using RestaurantApp.ViewModels;
using RestaurantApp.Views.Administrator.Dishes;
using RestaurantApp.Views.Administrator.Ingredients;
using RestaurantApp.Views.Administrator.Menus;
using System.Collections.ObjectModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RestaurantApp.Views.Administrator
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MenuAdministratorPage : ContentPage
    {
        //public ObservableCollection<Menu> Menus;
        MenuAdministratorViewModel viewModel;
        public static bool First = true;
        public MenuAdministratorPage()
        {
            InitializeComponent();
            BindingContext = viewModel = new MenuAdministratorViewModel();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            if (viewModel.Menus.Count == 0)
                viewModel.LoadItemsCommand.Execute(null);
        }

        private async void LvMenu_ItemSelected(object sender, SelectedItemChangedEventArgs args)
        {
            var menu = args.SelectedItem as Models.MainMenu;
            if (menu == null)
                return;

            switch (menu.Id)
            {
                case "Dishes":
                    await Navigation.PushAsync(new DishesAdministratorPage());
                    break;
                case "Ingredients":
                    await Navigation.PushAsync(new IngredientsAdministratorPage());
                    break;
                case "Menus":
                    await Navigation.PushAsync(new MenusAdministratorPage());
                    break;
                case "Calendars":
                    await Navigation.PushAsync(new DishCalendarAdministratorPage());
                    break;
                case "Deliverers":
                    await Navigation.PushAsync(new DeliverersAdministratorPage());
                    break;
            }

            // Manually deselect item.
            LvMenu.SelectedItem = null;
        }
    }
}