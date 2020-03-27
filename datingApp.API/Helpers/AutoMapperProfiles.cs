using System.Linq;
using AutoMapper;
using datingApp.API.DTOs;
using datingApp.API.Models;

namespace datingApp.API.Helpers
{
    public class AutoMapperProfiles : Profile
    {
        public AutoMapperProfiles()
        {
            CreateMap<User, UserForListDto>()
            .ForMember(member => member.photoUrl, opt => 
                opt.MapFrom(x => x.photos.FirstOrDefault(y => y.isMain).url))
            .ForMember(member => member.age,  opt => 
                opt.MapFrom(x => x.dateOfBirth.calculateAge()));

            CreateMap<User, UserForDetailDto>()
            .ForMember(member => member.photoUrl, opt => 
                opt.MapFrom(x => x.photos.FirstOrDefault(y => y.isMain).url))
            .ForMember(member => member.Age,  opt => 
                opt.MapFrom(x => x.dateOfBirth.calculateAge()));
            
            CreateMap<Photo, PhotoForDetailDto>();
            CreateMap<UserForUpdateDTO, User>();
            CreateMap<Photo, PhotoForReturnDto>();
            CreateMap<PhotoForCreationDto, Photo>();
            CreateMap<UserForRegisterDto, User>();
            CreateMap<MessageForCreationDto, Message>(); 
            CreateMap<Message, MessageToReturnDto>()
                .ForMember(m => m.senderPhotoUrl, opt => opt
                    .MapFrom(u=> u.sender.photos
                        .FirstOrDefault(p=>p.isMain).url))
                .ForMember(m => m.recipientPhotoUrl, opt => opt
                    .MapFrom(u=> u.recipient.photos
                        .FirstOrDefault(p=>p.isMain).url));
            
        }
    }
}