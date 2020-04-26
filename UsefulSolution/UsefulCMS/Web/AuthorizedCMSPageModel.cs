using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
