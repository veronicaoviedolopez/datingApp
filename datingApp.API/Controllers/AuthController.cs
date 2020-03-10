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
using AutoMapper;

namespace datingApp.API.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class AuthController : ControllerBase
  {
    private readonly IAuthRepository _repo;
    private readonly IConfiguration _config;
    private readonly IMapper _mapper;

    public AuthController(IAuthRepository repo, IConfiguration config, IMapper mapper)
    {
      _config = config;
      _mapper = mapper;
      _repo = repo;
    }
    [HttpPost("register")]
    public async Task<IActionResult> Register(UserForRegisterDto userForRegisterDto)
    {
      // validate request
      userForRegisterDto.Username = userForRegisterDto.Username.ToLower();
      if (await _repo.UserExists(userForRegisterDto.Username))
        return BadRequest("Username already exists");

      var userToCreate = _mapper.Map<User>(userForRegisterDto);

      var createdUser = await _repo.Register(userToCreate, userForRegisterDto.Password);

      var userToReturn = _mapper.Map<UserForDetailDto>(createdUser);
      return CreatedAtRoute("GetUser", new {controller= "User",
          id = createdUser.Id}, createdUser);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login(UserForLoginDto userFromLoginDto)
    {
      var userFromRepo = await _repo.Login(userFromLoginDto.Username.ToLower(), userFromLoginDto.Password);
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

      var user = _mapper.Map<UserForListDto>(userFromRepo);

      return Ok(new
      {
        token = tokenHandler.WriteToken(token),
        user
      });
    }
  }
}