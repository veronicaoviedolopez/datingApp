using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using datingApp.API.Data;
using datingApp.API.DTOs;
using datingApp.API.Helpers;
using datingApp.API.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace datingApp.API.Controllers
{
  [Authorize]
  [Route("api/user/{userid}/photos")]
  [ApiController]
  public class PhotoController : ControllerBase
  {
    private readonly IDatingRepository _repo;
    private readonly IMapper _mapper;
    private readonly IOptions<CloudinarySettings> _cloudinaryConfig;
    private Cloudinary _cloudinary;
    public PhotoController(IDatingRepository repo, IMapper mapper,
        IOptions<CloudinarySettings> cloudinaryConfig)
    {
      _cloudinaryConfig = cloudinaryConfig;
      _mapper = mapper;
      _repo = repo;

      Account acc = new Account(_cloudinaryConfig.Value.CloudName,
             _cloudinaryConfig.Value.ApiKey, _cloudinaryConfig.Value.ApiSecret);

      _cloudinary = new Cloudinary(acc);

    }

    [HttpGet("{id}", Name = "GetPhoto")]
    public async Task<IActionResult> GetPhoto(int id)
    {
      var photoFromRepo = await _repo.GetPhoto(id);
      var photo = _mapper.Map<PhotoForReturnDto>(photoFromRepo);
      return Ok(photo);
    }

    [HttpPost]
    public async Task<IActionResult> AddPhotoForUser(int userId, [FromForm] PhotoForCreationDto photoForCreationDto)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      var userFromRepo = await _repo.GetUser(userId);
      var file = photoForCreationDto.file;
      var uploadResult = new ImageUploadResult();
      if (file.Length > 0)
      {
        using (var stream = file.OpenReadStream())
        {
          var uploadParams = new ImageUploadParams()
          {
            File = new FileDescription(file.Name, stream),
            Transformation = new Transformation()
                    .Width(500).Height(500).Crop("fill").Gravity("face")
          };

          uploadResult = _cloudinary.Upload(uploadParams);
        }
      }
      photoForCreationDto.url = uploadResult.Uri.ToString();
      photoForCreationDto.publicId = uploadResult.PublicId;

      var photo = _mapper.Map<Photo>(photoForCreationDto);

      if (!userFromRepo.photos.Any(p => p.isMain))
      {
        photo.isMain = true;
      }

      userFromRepo.photos.Add(photo);

      if (await _repo.SaveAll())
      {
        var photoToReturn = _mapper.Map<PhotoForReturnDto>(photo);
        return CreatedAtRoute("GetPhoto",
            new { userid = userId, id = photo.id },
            photoToReturn);
      }

      return BadRequest("Could not add the photo");
    }

    [HttpPost("{id}/setMain")]
    public async Task<IActionResult> SetMainPhoto(int userId, int id)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      User user = await _repo.GetUser(userId);

      if (!user.photos.Any(x => x.id == id))
        return Unauthorized();

      Photo photoFromRepo = await _repo.GetPhoto(id);
      if (photoFromRepo.isMain)
        return BadRequest("This is already is the main photo");

      Photo mainPhotoFromRepo = await _repo.GetMainPhotoForUser(userId);
      mainPhotoFromRepo.isMain = false;
      photoFromRepo.isMain = true;

      if (await _repo.SaveAll())
        return NoContent();

      return BadRequest("Could not set main photo");
    }


    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePhoto(int userId, int id)
    {
      if (userId != int.Parse(User.FindFirst(ClaimTypes.NameIdentifier).Value))
        return Unauthorized();

      User user = await _repo.GetUser(userId);

      if (!user.photos.Any(x => x.id == id))
        return Unauthorized();

      Photo photoFromRepo = await _repo.GetPhoto(id);
      if (photoFromRepo.isMain)
        return BadRequest("You cannot delete your main photo");

      if (photoFromRepo.publicId != null)
      {
        var deletionParams = new DeletionParams(photoFromRepo.publicId);
        var deletionResult = _cloudinary.Destroy(deletionParams);

        if (deletionResult.Result == "ok")
        {
          _repo.Delete(photoFromRepo);
          if (await _repo.SaveAll())
            return Ok();
        }
        else
          return BadRequest("Failed to delete the photo" + deletionResult.Error);
      }
      _repo.Delete(photoFromRepo);
      if (await _repo.SaveAll())
        return Ok();

      return BadRequest("Failed to delete the photo");
    }
  }
}