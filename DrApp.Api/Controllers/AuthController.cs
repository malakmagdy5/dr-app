using DrApp.Api.Request;
using DrApp.Context.Entities.Users;
using DrApp.Context.YourNewFolderName;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace DrApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;
        private readonly PasswordHasher<Users> _passwordHasher = new();

        public AuthController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
                return BadRequest("Email and password are required.");

            var user = _context.Users.FirstOrDefault(u => u.Email == request.Email);
            if (user == null)
                return Unauthorized("Invalid email or password.");

            var result = _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, request.Password);
            if (result == PasswordVerificationResult.Failed)
                return Unauthorized("Invalid email or password.");

            bool isDoctor = _context.Doctor.Any(d => d.UserId == user.Id);
            bool isPatient = _context.Patient.Any(p => p.UserId == user.Id);

            return Ok(new
            {
                Message = "Login successful.",
                UserId = user.Id,
                Name = user.Name,
                Role = isDoctor ? "Doctor" : isPatient ? "Patient" : "Unknown"
            });
        }
    }
}

