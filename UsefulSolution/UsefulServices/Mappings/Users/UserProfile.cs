using AutoMapper;
using UsefulCore.Models.Users;
using UsefulDatabase.Model.Users;

namespace UsefulServices.Mappings.Users
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserSummary>();
        }
    }
}
