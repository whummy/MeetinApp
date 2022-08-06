using Application.DataTransferObjects;
using AutoMapper;
using Domain.Entities;
using Domain.Entities.Identities;

namespace Application.Mapper
{
    public class UserMapper : Profile
    {
        public UserMapper()
        {
            CreateMap<UserCreateDTO, User>();
            CreateMap<User, UserDTO>();
        }
    }
}