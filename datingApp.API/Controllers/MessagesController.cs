using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using datingApp.API.Data;
using datingApp.API.DTOs;
using datingApp.API.Helpers;
using datingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace datingApp.API.Controllers
{
  [ServiceFilter(typeof(LogUserActivity))]
  [Authorize]
  [Route("api/user/{userId}/[controller]")]
  [ApiController]
  public class MessagesController : ControllerBase
  {
    private readonly IDatingRepository _repo;
    private readonly IMapper _mapper;

    public MessagesController(IDatingRepository repo, IMapper mapper)
    {
      _repo = repo;
      _mapper = mapper;
    }

    [HttpGet("{id}", Name = "GetMessage")]
    public async Task<IActionResult> GetMessage(int userId, int id)
    {
        if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
        
        var messageFromRepo = _repo.GetMessage(id);

        if(messageFromRepo == null) 
            return NotFound();

        return Ok(messageFromRepo);
    }

    [HttpPost]
    public async Task<IActionResult> SetMessage(int userId, MessageForCreationDto messageForCreationDto)
    {
        if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

        messageForCreationDto.senderId= userId;

        var recipient = await _repo.GetUser(messageForCreationDto.recipientId);
        if (recipient == null)
            return BadRequest("Coul not find user");

        var message = _mapper.Map<Message>(messageForCreationDto);
        _repo.Add(message);

        if (await _repo.SaveAll())
        {
            var messageToReturn = _mapper.Map<MessageForCreationDto>(message);
            return CreatedAtRoute("GetMessage", 
                new {userId, id = message.id}, messageToReturn);
        }

        throw new System.Exception("Creating the message failed to save");
    }
  }
}