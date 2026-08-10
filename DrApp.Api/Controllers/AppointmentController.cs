using DrApp.Api.Request;
using DrApp.Context.Entities.Users;
using DrApp.Context.YourNewFolderName;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DrApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AppointmentController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;

        public AppointmentController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Book")]
        public IActionResult Book([FromBody] AppointmentRequest request)
        {
            bool patientExists = _context.Patient.Any(p => p.Id == request.PatientId);
            if (!patientExists)
                return BadRequest("Patient not found.");

            bool doctorExists = _context.Doctor.Any(d => d.Id == request.DoctorId);
            if (!doctorExists)
                return BadRequest("Doctor not found.");

            var requestedDay = request.AppointmentDate.DayOfWeek;
            var requestedTime = request.AppointmentDate.TimeOfDay;

            bool isWithinAvailability = _context.DoctorAvailability.Any(a =>
                a.DoctorId == request.DoctorId &&
                a.DayOfWeek == requestedDay &&
                requestedTime >= a.StartTime &&
                requestedTime < a.EndTime);

            if (!isWithinAvailability)
                return BadRequest("Doctor is not available at this date/time.");

            bool slotTaken = _context.Appointment.Any(a =>
                a.DoctorId == request.DoctorId &&
                a.AppointmentDate == request.AppointmentDate &&
                a.Status != "Cancelled");

            if (slotTaken)
                return BadRequest("This time slot is already booked.");

            var appointment = new Appointment
            {
                PatientId = request.PatientId,
                DoctorId = request.DoctorId,
                AppointmentDate = request.AppointmentDate,
                Status = "Pending"
            };

            _context.Appointment.Add(appointment);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Appointment booked successfully.",
                AppointmentId = appointment.Id
            });
        }

        [HttpGet("Patient/{patientId}")]
        public IActionResult GetPatientAppointments(int patientId)
        {
            bool patientExists = _context.Patient.Any(p => p.Id == patientId);
            if (!patientExists)
                return BadRequest("Patient not found.");

            var appointments = _context.Appointment
                .Include(a => a.Doctor)
                    .ThenInclude(d => d.Users)
                .Where(a => a.PatientId == patientId)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new
                {
                    a.Id,
                    DoctorName = a.Doctor.Users.Name,
                    a.AppointmentDate,
                    a.Status
                })
                .ToList();

            return Ok(appointments);
        }

        [HttpGet("Doctor/{doctorId}")]
        public IActionResult GetDoctorAppointments(int doctorId)
        {
            bool doctorExists = _context.Doctor.Any(d => d.Id == doctorId);
            if (!doctorExists)
                return BadRequest("Doctor not found.");

            var appointments = _context.Appointment
                .Include(a => a.Patient)
                    .ThenInclude(p => p.Users)
                .Where(a => a.DoctorId == doctorId)
                .OrderByDescending(a => a.AppointmentDate)
                .Select(a => new
                {
                    a.Id,
                    PatientName = a.Patient.Users.Name,
                    a.AppointmentDate,
                    a.Status
                })
                .ToList();

            return Ok(appointments);
        }

        [HttpPut("Cancel/{id}")]
        public IActionResult Cancel(int id)
        {
            var appointment = _context.Appointment.FirstOrDefault(a => a.Id == id);
            if (appointment == null)
                return NotFound("Appointment not found.");

            if (appointment.Status == "Cancelled")
                return BadRequest("This appointment is already cancelled.");

            if (appointment.Status == "Completed")
                return BadRequest("Cannot cancel a completed appointment.");

            appointment.Status = "Cancelled";
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Appointment cancelled successfully.",
                AppointmentId = appointment.Id
            });
        }
    }
}