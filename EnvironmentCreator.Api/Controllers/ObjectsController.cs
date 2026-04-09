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
    [Route("environments/{environmentId}/objects")]
    [Authorize]
    public class ObjectsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ObjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetObjects(int environmentId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var environment = await _context.Environments2D
                .FirstOrDefaultAsync(e => e.Id == environmentId && e.UserId == userId);

            if (environment == null)
                return NotFound("Environment niet gevonden");

            var objects = await _context.Objects2D
                .Where(o => o.Environment2DId == environmentId)
                .ToListAsync();

            return Ok(objects);
        }

        [HttpPost]
        public async Task<IActionResult> AddObject(int environmentId, CreateObject2DDto dto)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var environment = await _context.Environments2D
                .FirstOrDefaultAsync(e => e.Id == environmentId && e.UserId == userId);

            if (environment == null)
                return NotFound("Environment niet gevonden");

            var obj = new Object2D
            {
                PrefabId = dto.PrefabId,
                PositionX = dto.PositionX,
                PositionY = dto.PositionY,
                ScaleX = dto.ScaleX,
                ScaleY = dto.ScaleY,
                RotationZ = dto.RotationZ,
                SortingLayer = dto.SortingLayer,
                Environment2DId = environmentId
            };

            _context.Objects2D.Add(obj);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                obj.Id,
                obj.PrefabId,
                obj.PositionX,
                obj.PositionY,
                obj.ScaleX,
                obj.ScaleY,
                obj.RotationZ,
                obj.SortingLayer,
                obj.Environment2DId
            });
        }

        
        [HttpDelete("{objectId}")]
        public async Task<IActionResult> DeleteObject(int environmentId, int objectId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var environment = await _context.Environments2D
                .FirstOrDefaultAsync(e => e.Id == environmentId && e.UserId == userId);

            if (environment == null)
                return NotFound("Environment niet gevonden");

            var obj = await _context.Objects2D
                .FirstOrDefaultAsync(o => o.Id == objectId && o.Environment2DId == environmentId);

            if (obj == null)
                return NotFound("Object niet gevonden");

            _context.Objects2D.Remove(obj);
            await _context.SaveChangesAsync();

            return Ok("Object verwijderd");
        }
    }
}