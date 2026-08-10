using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace DrApp.Context.Entities.Users
{
    public class DoctorAvailability
    {
        public int Id { get; set; }

        public int DoctorId { get; set; }
        [ForeignKey(nameof(DoctorId))]
        public Doctor Doctor { get; set; }

        public DayOfWeek DayOfWeek { get; set; }   // e.g. Monday
        public TimeSpan StartTime { get; set; }    // e.g. 09:00
        public TimeSpan EndTime { get; set; }       // e.g. 17:00
    }
}