using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model.Users;
using static UsefulCore.Constants.Roles.RoleConstants;

namespace UsefulCMS.Web
{
    [Authorize(Roles = Admin)]
    public abstract class AuthorizedCMSPageModel : CMSPageModel
    {
        public AuthorizedCMSPageModel(IMapper mapper) : base(mapper)
        {
        }
    }
}
