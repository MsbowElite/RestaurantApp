using RestaurantApp.Models;
using RestaurantApp.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public class MenuAdministratorMock : IMenuAdministrator<MainMenu>
    {
        List<MainMenu> menus;

        public MenuAdministratorMock()
        {
            menus = new List<MainMenu>
            {
                new MainMenu { Id = "Menus", Name = "Menus", Image = "lol" },
                new MainMenu { Id = "Dishes", Name = "Pratos", Image = "lol" },
                new MainMenu { Id = "Ingredients", Name = "Ingredientes", Image = "lol" },
                new MainMenu { Id = "Calendars", Name = "Calendario", Image = "lol" },
                new MainMenu { Id = "Deliverers", Name = "Entregadores", Image = "lol" },
            };
        }

        public async Task<IEnumerable<MainMenu>> GetItemsAsync(bool forceRefresh = false)
        {
            return await Task.FromResult(menus);
        }
    }
}
