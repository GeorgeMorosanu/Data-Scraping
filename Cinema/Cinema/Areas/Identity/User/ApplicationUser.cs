using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Cinema.Areas.Identity.User
{
    public class ApplicationUser : IdentityUser
    {
        //Id [Could be transformed to Kennitala], UserName, Email, PhoneNumber
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public DateTime BirthDay { get; set; }

        // 0 - Female, 1 - Male
        public bool Gender { get; set; }

        // Region = City
        public string Region { get; set; }

        public string Country { get; set; }

        public bool ActiveAccount { get; set; }

        public DateTime CreatedDate { get; set; }

        //OnCreate, Rewards . Add -> (User, CreatedDate)
    }
}
