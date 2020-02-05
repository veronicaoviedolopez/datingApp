using System.Threading.Tasks;
using datingApp.API.Data;
using datingApp.API.Models;
using Microsoft.AspNetCore.Mvc;
using datingApp.API.DTOs;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System;
using System.IdentityModel.Tokens.Jwt;

namespace datingApp.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController: ControllerBase
    {
        private readonly IAuthRepository _repo;
        private readonly IConfiguration _config;
        public AuthController(IAuthRepository repo, IConfiguration config)
        {
            _config = config;
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

        [HttpPost("login")]
        public async Task<IActionResult> Login(UserForLoginDTO userFromLoginDto) {
            var userFromRepo =  await _repo.Login(userFromLoginDto.Username.ToLower(), userFromLoginDto.Password);
            if (userFromRepo == null)
                return Unauthorized();
            
            var claims = new[] 
            {
                new Claim(ClaimTypes.NameIdentifier, userFromRepo.Id.ToString()),
                new Claim(ClaimTypes.Name, userFromRepo.Username)
            };
            
            var key = new SymmetricSecurityKey(Encoding.UTF8
                .GetBytes(_config.GetSection("AppSettings:Token").Value));

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.Now.AddDays(1),
                SigningCredentials = creds
            };

            var tokenHandler = new JwtSecurityTokenHandler();

            var token = tokenHandler.CreateToken(tokenDescriptor);

            return Ok(new {
                token = tokenHandler.WriteToken(token)        
            });
        }
    }
}