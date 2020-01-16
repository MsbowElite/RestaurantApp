using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class DishCalendar
    {
        public string DishId { get; set; }
        public string CompanyId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
    }
}
