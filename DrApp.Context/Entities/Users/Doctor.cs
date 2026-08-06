using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrApp.Context.Entities.Users
{
    public class Doctor
    {
        //Make relation between doctor and users entity [foreign key]
        public int Id { get; set; }
        public int UserId { get; set; }

        [ForeignKey(nameof(UserId))]

        public Users Users { get; set; }
        public string LiscenceNumber { get; set; }

 
        public string Degree { get; set; }

        public string Biography { get; set; }
   
    }
}
