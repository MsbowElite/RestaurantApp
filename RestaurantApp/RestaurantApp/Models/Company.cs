using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class Company
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public bool Active { get; set; }
        public byte MenuOption { get; set; }
    }
}
