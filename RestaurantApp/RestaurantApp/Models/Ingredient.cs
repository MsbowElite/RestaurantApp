using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class Ingredient
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
        public string Description { get; set; }
    }
}
