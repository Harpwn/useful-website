using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using UsefulDatabase.Model;
using static UsefulCore.Constants.Roles.RoleConstants;

namespace UsefulCMS.Models
{
    [Authorize(Roles = Admin)]
    public abstract class AuthorizedCMSPageModel : CMSPageModel
    {
        public AuthorizedCMSPageModel(IMapper mapper) : base(mapper)
        {
        }
    }
}
