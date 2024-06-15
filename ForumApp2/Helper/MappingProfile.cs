using AutoMapper;
using ForumApp2.DTOs;
using ForumApp2.Models;

namespace ForumApp2.Helper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<ApplicationUser, RegisterUserDto>().ReverseMap();
            CreateMap<Topic, TopicDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<ApplicationUser, LoginUserDto>().ReverseMap();

            CreateMap<LoginUserDto, ApplicationUser>();
            CreateMap<RegisterUserDto, ApplicationUser>();
            CreateMap<TopicDto, Topic>();
            CreateMap<CommentDto, Comment>();
        }
    }
}
