using DrApp.Context.Entities.Users;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DrApp.Context.Entities
{
    public class DoctorSpecialization
    {
        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }

        public int SpecializationId { get; set; }
        [ForeignKey(nameof(SpecializationId))]
        public Specialization Specialization { get; set; }
    }
}
