using AutoMapper;
using Forum_Application_API.Dto;
using Forum_Application_API.Models;

namespace Forum_Application_API
{
    public class Mapper : Profile
    {
        public Mapper()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();
            CreateMap<LoginUserDto, User>();
            CreateMap<UpdateUserDto, User>();
            CreateMap<ForumThread, ThreadDto>();
            CreateMap<ThreadDto, ForumThread>();
            CreateMap<Comment, CommentDto>();
            CreateMap<CommentDto, Comment>();
        }
    }
}
