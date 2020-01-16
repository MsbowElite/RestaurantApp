using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class UserAuthModel
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Username { get; set; }
        public string Token { get; set; }
        public IEnumerable<UserRole> UserRoles { get; set; }
    }
}
