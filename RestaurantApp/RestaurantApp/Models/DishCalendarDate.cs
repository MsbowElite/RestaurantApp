using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class DishCalendarDate
    {
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Dish Dish { get; set; }
    }
}
