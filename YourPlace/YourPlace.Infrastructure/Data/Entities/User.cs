using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace YourPlace.Infrastructure.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }

        
        public string Surname { get; set; }

        
        public string Email { get; set; }

        
        public string Password { get; set; }

        
        public string Role { get; set; }

        public User() : base()
        {

        }
    }
}
