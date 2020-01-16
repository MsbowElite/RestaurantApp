using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantApp.Services
{
    public interface IMenuAdministrator<T>
    {
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}
