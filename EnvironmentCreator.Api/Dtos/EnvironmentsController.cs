using EnvironmentCreator.Api.Data;
using EnvironmentCreator.Api.Dtos;
using EnvironmentCreator.Api.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace EnvironmentCreator.Api.Controllers
{
    [ApiController]
    [Route("environments")]
    [Authorize]
    public class EnvironmentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public EnvironmentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetMyEnvironments()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var environments = await _context.Environments2D
                .Where(e => e.UserId == userId)
                .Select(e => new
                {
                    e.Id,
                    e.Name
                })
                .ToListAsync();

            return Ok(environments);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetEnvironmentById(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var environment = await _context.Environments2D
                .Include(e => e.Objects)
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (environment == null)
            {
                return NotFound("Environment niet gevonden");
            }

            return Ok(environment);
        }

        [HttpPost]
        public async Task<IActionResult> CreateEnvironment(CreateEnvironmentDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var environmentCount = await _context.Environments2D
                .CountAsync(e => e.UserId == userId);

            if (environmentCount >= 5)
            {
                return BadRequest("Je mag maximaal 5 environments hebben");
            }

            var nameExists = await _context.Environments2D
                .AnyAsync(e => e.UserId == userId && e.Name == dto.Name);

            if (nameExists)
            {
                return BadRequest("Je hebt al een environment met deze naam");
            }

            var environment = new Environment2D
            {
                Name = dto.Name,
                UserId = userId!
            };

            _context.Environments2D.Add(environment);
            await _context.SaveChangesAsync();

            return Ok(environment);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnvironment(int id)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var environment = await _context.Environments2D
                .FirstOrDefaultAsync(e => e.Id == id && e.UserId == userId);

            if (environment == null)
            {
                return NotFound("Environment niet gevonden");
            }

            _context.Environments2D.Remove(environment);
            await _context.SaveChangesAsync();

            return Ok("Environment verwijderd");
        }
    }
}