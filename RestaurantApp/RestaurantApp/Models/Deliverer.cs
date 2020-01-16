using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class Deliverer
    {
        public Guid? Id { get; set; }
        public string Name { get; set; }
        public Guid CompanyId { get; set; }
    }
}
