using AutoMapper;
using UsefulDatabase.Model.Users;
using UsefulServices.Dtos.Users;

namespace UsefulServices.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>();
        }
    }
}
