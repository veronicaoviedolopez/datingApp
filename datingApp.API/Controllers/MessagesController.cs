using System;
using System.Collections.Generic;
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
        
        var messageFromRepo = await _repo.GetMessage(id);

        if(messageFromRepo == null) 
            return NotFound();

        return Ok(messageFromRepo);
    }

    [HttpGet]
    public async Task<IActionResult> GetMessagesForUser(int userId, [FromQuery]MessageParams messageParams)
    {
        if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

        messageParams.UserId = userId;
        
        var messagesFromRepo = await _repo.GetMessagesForUser(messageParams);
        
        var messages = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesFromRepo);
        
        Response.AddPagination(messagesFromRepo.CurrrentPage, messagesFromRepo.PageSize,
            messagesFromRepo.TotalCount, messagesFromRepo.TotalPages);

        return Ok(messages);
    }

    [HttpGet("thread/{recipientId}")]
    public async Task<IActionResult> GetMessageThread(int userId, int recipientId)
    {
        if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();
        
        var messagesfromRepo = await _repo.GetMessageThread(userId, recipientId);

        var messageThread = _mapper.Map<IEnumerable<MessageToReturnDto>>(messagesfromRepo);

        return Ok(messageThread);
    }

    [HttpPost]
    public async Task<IActionResult> CreateMessage(int userId, MessageForCreationDto messageForCreationDto)
    {
        User sender = await _repo.GetUser(userId);
        if (sender.Id != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

        messageForCreationDto.senderId= userId;

        var recipient = await _repo.GetUser(messageForCreationDto.recipientId);
        if (recipient == null)
            return BadRequest("Could not find user");

        var message = _mapper.Map<Message>(messageForCreationDto);
        _repo.Add(message);

        if (await _repo.SaveAll())
        {
            var messageToReturn = _mapper.Map<MessageToReturnDto>(message);
            return CreatedAtRoute("GetMessage", 
                new {userId, id = message.id}, messageToReturn);
        }

        throw new System.Exception("Creating the message failed to save");
    }

    [HttpDelete("{messageId}")]
    public async Task<IActionResult> DeleteMessage(int userId, int messageId){
        if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

        var messageFromRepo = await _repo.GetMessage(messageId);
        if (messageFromRepo == null)
            return BadRequest("message does not exists");

        if (messageFromRepo.senderId == userId)
            messageFromRepo.senderDeleted = true;
        
        if (messageFromRepo.recipientId == userId)
            messageFromRepo.recipientDeleted = true;

        if (messageFromRepo.senderDeleted &&  messageFromRepo.recipientDeleted)
            _repo.Delete(messageFromRepo);
        
        if (await _repo.SaveAll())
            return NoContent();

        throw new System.Exception("Error deleting the message");
    }

    [HttpPost("{messageId}/read")]
    public async Task<IActionResult> MarkAsReadMessage(int userId, int messageId) {
         if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
            return Unauthorized();

        var messageFromRepo = await _repo.GetMessage(messageId);
        
        if (messageFromRepo == null)
            return BadRequest("message does not exists");
        
        if (messageFromRepo.recipientId != userId)
            return Unauthorized();

        messageFromRepo.isRead = true;
        messageFromRepo.dateRead = DateTime.Now;
        
         if (await _repo.SaveAll())
            return NoContent();

        throw new System.Exception("Error marking as read the message");
    }
  }
}