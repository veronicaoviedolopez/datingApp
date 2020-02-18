using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using datingApp.API.Data;
using datingApp.API.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace datingApp.API.Controllers
{
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

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(int id)
    {
        var user = await _repo.GetUser(id);
        var userToReturn = _mapper.Map<UserForDetailDto>(user);
        return Ok(userToReturn);
    }
  }
}