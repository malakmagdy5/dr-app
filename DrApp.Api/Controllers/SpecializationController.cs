using DrApp.Context.YourNewFolderName;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq;   // ADD THIS — needed for .Where()

namespace DrApp.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpecializationController : ControllerBase
    {
        private readonly DoctorAppDbContext _context;

        public SpecializationController(DoctorAppDbContext context)
        {
            _context = context;
        }

        [HttpGet("GetAll")]
        public IActionResult GetAll()
        {
            var specializations = _context.Specializations.ToList();
            return Ok(specializations);
        }

        [HttpGet("Search")]
        public IActionResult Search([FromQuery] string query)
        {
            if (string.IsNullOrWhiteSpace(query))
            {
                return Ok(_context.Specializations.ToList());
            }

            var results = _context.Specializations
                .Where(s => s.Name.Contains(query))
                .ToList();

            return Ok(results);
        }
    }
}
