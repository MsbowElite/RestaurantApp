using System;
using System.Collections.Generic;
using System.Text;

namespace RestaurantApp.Models
{
    public class TokenPayload
    {
        public string Role { get; set; }
        public int Nbf { get; set; }
        public int Exp { get; set; }
        public int Iat { get; set; }
    }
}
