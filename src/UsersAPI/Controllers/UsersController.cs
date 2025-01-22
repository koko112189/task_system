using Application.DTOs;
using Application.Interfaces;
using Domain.Exceptions.UserExceptions;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService spaceService)
        {
            _userService = spaceService;
        }

        [HttpGet]
        public async Task<IActionResult> GetUsers()
        {
            var spaces = await _userService.GetUserAsync();
            return Ok(spaces);
        }

        [HttpPost]
        public async Task<IActionResult> CreateUser([FromBody] CreateUserDto user)
        {
            if (user == null)
            {
                return BadRequest("User data is invalid.");
            }
            var newUser = new User
            {
                Name = user.Name,
                Email = user.Email,
            };
            try
            {
                await _userService.CreateUserAsync(newUser);
                return Ok(user);
            }
            catch(UserAlreadyExistsException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Ocurrió un error inesperado.");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(Guid id, [FromBody] UserDto user)
        {
            if (id != user.Id)
            {
                return BadRequest("User ID mismatch.");
            }
            var updateUser = new User
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email
            };
            try
            {
                await _userService.UpdateUserAsync(updateUser);
                return Ok($"Espacio con ID: {user.Id} actualizado exitosamente.");
            }
            catch (UserNotFoundException)
            {
                return NotFound("User not found.");
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(Guid id)
        {
            try
            {
                await _userService.DeleteUserAsync(id);
            }
            catch (UserNotFoundException)
            {
                return NotFound("User not found.");
            }

            return Ok($"Espacio con ID: {id} eliminado exitosamente.");
        }
    }
}
