using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrApp.Context.Entities
{
    public class Specialization
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public ICollection<DoctorSpecialization> DoctorSpecializations { get; set; }
    }
}
