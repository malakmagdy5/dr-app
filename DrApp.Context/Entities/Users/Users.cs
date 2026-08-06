using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrApp.Context.Entities.Users
{
    public class Users
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Email { get; set; }

        public string PhoneNumber { get; set; }

        public string Address { get; set; }

        public int Gender { get; set; }

        public string PasswordHash { get; set; }

        public Patient? Patient { get; set; }

        public Doctor? Doctor { get; set; }


        //Make relation between patient and users , Doctor , Patient entity [foreign key]
    }
}
