using RestaurantApp.Models;
using RestaurantApp.Services;
using RestaurantApp.Services.Interfaces;
using RestaurantApp.Views.Administrator.Dishes;
using RestaurantApp.Views.Administrator.Ingredients;
using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace RestaurantApp.ViewModels.Dishes
{
    public class NewDishCalendarAdministratorViewModel : BaseViewModel<DishCalendar>
    {
        public DishCalendar Item { get; set; }
        public DateTime Date { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime MinDate { get; set; }
        public DateTime MaxDate { get; set; }
        public Dish Dish { get; set; }
        public bool New { get; set; }
        public bool NewInverted
        {
            get
            {
                return !New;
            }
        }
        public ICompanyDataStore DataStore => DependencyService.Get<ICompanyDataStore>();
        public NewDishCalendarAdministratorViewModel(DateTime? date = null)
        {
            //if (date != null)
            //{
            //    Title = "Editar Prato";
            //    Item = item;
            //    LoadItemCommand = new Command(async () => await ExecuteLoadItemCommand());
            //}
            //else
            //{
            //    Title = "Novo Prato";
            //    Item = new Dish();
            //    New = true;
            //}
            Dish = new Dish
            {
                Name = "Selecione um Prato! Clique aqui."
            };
            Item = new DishCalendar();

            MinDate = Date = StartTime = EndTime = DateTime.UtcNow;
            MaxDate = MinDate.AddMonths(1);

            if (date.HasValue)
            {
                Item.EndTime = Item.StartTime = date.Value;
            }

            MessagingCenter.Subscribe<DishesAdministratorPage, Dish>(this, "SelectDish", async (obj, dish) =>
            {
                if (dish != null)
                    SetupDish(dish as Dish);
            });
        }

        private void SetupDish(Dish dish)
        {
            Dish = dish;
            Item.DishId = Dish.Id;
            Item.CompanyId = Dish.CompanyId;
            OnPropertyChanged("Dish");
        }

        public async Task<DishCalendar> Add()
        {
            Item.StartTime = new DateTime(Date.Year, Date.Month, Date.Day, StartTime.Hour, StartTime.Minute, 0);
            Item.EndTime = new DateTime(Date.Year, Date.Month, Date.Day, EndTime.Hour, EndTime.Minute, 0);

            Item = await DataStore.AddDishCalendarsOwn(Item);

            if (Item != null && string.IsNullOrEmpty(Item.DishId))
            {
                return Item;
            }

            return null;
        }
    }
}
