using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using datingApp.API.Data;
using datingApp.API.DTOs;
using datingApp.API.Helpers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace datingApp.API.Controllers
{
  [ServiceFilter(typeof(LogUserActivity))]
  [Authorize]
  [Route("api/[controller]")]
  [ApiController]
  public class UserController : ControllerBase
  {
    private readonly IMapper _mapper;
    private readonly IDatingRepository _repo;

    public UserController(IDatingRepository repo, IMapper mapper)
    {
      _mapper = mapper;
      _repo = repo;
    }

    [HttpGet]
    public async Task<IActionResult> GetUsers() 
    {
        var users = await _repo.GetUsers();
        var usersToReturn = _mapper.Map<IEnumerable<UserForListDto>>(users);
        return Ok(usersToReturn);
    }

    [HttpGet("{id}", Name="GetUser")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _repo.GetUser(id);
        var userToReturn = _mapper.Map<UserForDetailDto>(user);
        return Ok(userToReturn);
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBodyAttribute]UserForUpdateDTO userForUpdateDto) {
      var borrame = User.FindFirst(ClaimTypes.NameIdentifier).Value;
      if ( id != int.Parse(borrame)) {
        return Unauthorized();
      } 
      
      var userFromRepo = await _repo.GetUser(id);
      _mapper.Map(userForUpdateDto, userFromRepo);

      if(await _repo.SaveAll())
        return Ok(userForUpdateDto);

      throw new System.Exception($"Updating user {id} failed on save");
    }
  }
}