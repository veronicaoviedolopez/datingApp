using System.Threading.Tasks;
using datingApp.API.Data;
using datingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using datingApp.API.DTOs;

namespace datingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        public AuthController(IAuthRepository repo)
        {
            _repo = repo;
        }
        [HttpPost]
        public async Task<IActionResult> Register(UserToRegisterDto userToRegisterDto) {
            // validate request
            userToRegisterDto.Username =userToRegisterDto.Username.ToLower();
            if( await _repo.UserExists(userToRegisterDto.Username))
                return BadRequest("Username already exists");
            
            var userToCreate = new User {
                Username = userToRegisterDto.Username
            };

            var createdUser = await _repo.Register(userToCreate, userToRegisterDto.Password);
            
            return StatusCode(201);
        }
    }
}