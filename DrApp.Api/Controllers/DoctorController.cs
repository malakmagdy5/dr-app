using DrApp.Api.Request;
using DrApp.Context.Entities.Users;
using DrApp.Context.YourNewFolderName;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace DrApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;
        private readonly PasswordHasher<Users> _passwordHasher = new();

        public DoctorController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterDoctorRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Name))
                return BadRequest("Name is required.");

            if (string.IsNullOrWhiteSpace(request.Email))
                return BadRequest("Email is required.");

            if (_context.Users.Any(x => x.Email == request.Email))
                return BadRequest("Email already exists.");

            if (string.IsNullOrWhiteSpace(request.PhoneNumber))
                return BadRequest("Phone number is required.");

            if (_context.Users.Any(x => x.PhoneNumber == request.PhoneNumber))
                return BadRequest("Phone number already exists.");

            if (string.IsNullOrWhiteSpace(request.PasswordHash))
                return BadRequest("Password is required.");

            if (request.PasswordHash.Length < 8)
                return BadRequest("Password must be at least 8 characters.");

            if (string.IsNullOrWhiteSpace(request.Address))
                return BadRequest("Address is required.");

            if (string.IsNullOrWhiteSpace(request.LiscenceNumber))
                return BadRequest("License number is required.");

            if (string.IsNullOrWhiteSpace(request.Degree))
                return BadRequest("Degree is required.");

            if (string.IsNullOrWhiteSpace(request.Biography))
                return BadRequest("Biography is required.");

            var user = new Users
            {
                Name = request.Name,
                Email = request.Email,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                Gender = request.Gender
            };
            user.PasswordHash = _passwordHasher.HashPassword(user, request.PasswordHash);

            _context.Users.Add(user);
            _context.SaveChanges();

            var doctor = new Doctor
            {
                UserId = user.Id,
                LiscenceNumber = request.LiscenceNumber,
                Degree = request.Degree,
                Biography = request.Biography
            };
            _context.Doctor.Add(doctor);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Doctor registered successfully.",
                DoctorId = doctor.Id,
                UserId = user.Id
            });
        }

        [HttpGet("GetBySpecialization")]
        public IActionResult GetBySpecialization([FromQuery] int? specializationId, [FromQuery] string? name)
        {

            try
            {
                var query = _context.DoctorSpecializations.Include(c => c.Doctor).ThenInclude(c => c.Users)
                                                          .Include(c => c.Specialization)
                                                          .Where(c => c.SpecializationId == specializationId);

                if (!string.IsNullOrWhiteSpace(name))
                {
                    query = query.Where(d => d.Doctor.Users.Name.Contains(name));
                }

                var result = query.Select(c => new
                {

                    Name = c.Doctor.Users.Name,
                    Specialization = c.Specialization.Name,
                    LiscenceNumber = c.Doctor.LiscenceNumber,
                }).ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {

                throw;
            }
        }
    }
}
