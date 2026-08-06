using DrApp.Api.Request;
using DrApp.Context.Entities.Users;
using DrApp.Context.YourNewFolderName;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Numerics;

namespace DrApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;
        private readonly PasswordHasher<Users> _passwordHasher = new();

        public PatientController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("register")]
        public IActionResult Register([FromBody] RegisterPatientRequest request)
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

            if (string.IsNullOrWhiteSpace(request.BloodType))
                return BadRequest("Blood type is required.");
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

            var patient = new Patient
            {
                UserId = user.Id,
                BloodType = request.BloodType
            };
            _context.Patient.Add(patient);
            _context.SaveChanges();

            return Ok(new
            {
                Message = "Patient registered successfully.",
                PatientId = patient.Id,
                UserId = user.Id
            });
        }
    }
}
