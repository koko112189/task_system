using Application.DTOs;
using Application.Interfaces;
using Domain.Exceptions.SpaceExceptions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace SpaceManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SpacesController : ControllerBase
    {
        private readonly ISpaceService _spaceService;

        public SpacesController(ISpaceService spaceService)
        {
            _spaceService = spaceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetSpaces()
        {
            var spaces = await _spaceService.GetSpacesAsync();
            return Ok(spaces);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSpace([FromBody] CreateSpaceDto space)
        {
            if (space == null)
            {
                return BadRequest("Space data is invalid.");
            }
            var newSpace = new Space
            {
                Name = space.Name,
                Capacity = space.Capacity
            };
            try
            {
                await _spaceService.CreateSpaceAsync(newSpace);
                return Ok(space);
            }
            catch(SpaceAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateSpace(Guid id, [FromBody] SpaceDto space)
        {
            if (id != space.Id)
            {
                return BadRequest("Space ID mismatch.");
            }
            var updateSpace = new Space
            {
                Id = space.Id,
                Name = space.Name,
                Capacity = space.Capacity
            };
            try
            {
                await _spaceService.UpdateSpaceAsync(updateSpace);
                return Ok($"Espacio con ID: {space.Id} actualizado exitosamente.");
            }
            catch (SpaceNotFoundException)
            {
                return NotFound("Space not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSpace(Guid id)
        {
            try
            {
                await _spaceService.DeleteSpaceAsync(id);
            }
            catch (SpaceNotFoundException)
            {
                return NotFound("Space not found.");
            }

            return Ok($"Espacio con ID: {id} eliminado exitosamente.");
        }
    }
}
