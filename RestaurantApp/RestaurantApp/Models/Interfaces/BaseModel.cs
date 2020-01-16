using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class BaseModel<T>
    {
        string Id { get; set; }
        T Model { get; set; }
    }
}
