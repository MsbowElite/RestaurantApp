using RestaurantApp.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface ICompanyDataStore : IDataStore<Company>
    {
        Task<IEnumerable<Company>> GetMyOwnListAsync(bool forceRefresh = false);
        Task<IEnumerable<DishCalendar>> GetDishCalendarsOwnMonth(string companyId, byte month, int year);
        Task<IEnumerable<DishCalendarDate>> GetDishCalendarsOwnDate(string companyId, long dateTicks);
        Task<bool> RemoveDishCalendarsOwn(DishCalendar item, long dateTicks);
        Task<DishCalendar> AddDishCalendarsOwn(DishCalendar dishCalendar);
    }
}
