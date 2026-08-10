using DrApp.Api.Request;
using DrApp.Context.Entities.Users;
using DrApp.Context.YourNewFolderName;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DrApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorAvailabilityController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;

        public DoctorAvailabilityController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("Set")]
        public IActionResult Set([FromBody] AvailabilityRequest request)
        {
            bool doctorExists = _context.Doctor.Any(d => d.Id == request.DoctorId);
            if (!doctorExists)
                return BadRequest("Doctor not found.");

            if (request.StartTime >= request.EndTime)
                return BadRequest("Start time must be before end time.");

            var availability = new DoctorAvailability
            {
                DoctorId = request.DoctorId,
                DayOfWeek = request.DayOfWeek,
                StartTime = request.StartTime,
                EndTime = request.EndTime
            };

            _context.DoctorAvailability.Add(availability);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Availability added successfully.",
                AvailabilityId = availability.Id
            });
        }

        [HttpGet("Doctor/{doctorId}")]
        public IActionResult GetByDoctor(int doctorId)
        {
            var availability = _context.DoctorAvailability
                .Where(a => a.DoctorId == doctorId)
                .OrderBy(a => a.DayOfWeek)
                .ThenBy(a => a.StartTime)
                .ToList();

            return Ok(availability);
        }
    }
}