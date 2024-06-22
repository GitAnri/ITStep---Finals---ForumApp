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
            CreateMap<Topic, TopicPostDto>().ReverseMap();
            CreateMap<Topic, TopicGetDto>().ReverseMap();
            CreateMap<Comment, CommentDto>().ReverseMap();
            CreateMap<Comment, CommentUpdateDto>().ReverseMap();
            CreateMap<Comment, CommentForGettingDto>().ReverseMap();
            CreateMap<ApplicationUser, LoginUserDto>().ReverseMap();

            CreateMap<LoginUserDto, ApplicationUser>();
            CreateMap<RegisterUserDto, ApplicationUser>();
            CreateMap<TopicPostDto, Topic>();
            CreateMap<TopicGetDto, Topic>();
            CreateMap<CommentDto, Comment>();
            CreateMap<CommentForGettingDto, Comment>();
            CreateMap<CommentUpdateDto, Comment>().ReverseMap();
        }
    }
}
