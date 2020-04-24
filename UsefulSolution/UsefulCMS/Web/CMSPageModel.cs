using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.RazorPages;
using static UsefulCore.Constants.Roles.RoleConstants;

namespace UsefulCMS.Web
{
    //[Authorize(Roles = Admin)]
    public abstract class CMSPageModel : PageModel
    {
        internal IMapper _mapper;
        public CMSPageModel(IMapper mapper)
        {
            _mapper = mapper;
        }

    }
}
