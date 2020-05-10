using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace UsefulCMS.Models
{
    public abstract class CMSPageModel : PageModel
    {
        protected readonly IMapper Mapper;

        public CMSPageModel(IMapper mapper)
        {
            Mapper = mapper;
        }

    }
}
