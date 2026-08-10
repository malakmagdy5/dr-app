using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrApp.Context.Entities.Users
{
    public class Appointment
    {
        public int Id { get; set; }

        public int PatientId { get; set; }
        [ForeignKey(nameof(PatientId))]
        public Patient Patient { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }

        public DateTime AppointmentDate { get; set; }
        public string Status { get; set; } = "Pending"; // Pending, Confirmed, Cancelled, Completed
    }
}