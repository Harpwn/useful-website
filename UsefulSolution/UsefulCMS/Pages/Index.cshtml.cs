using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsefulCMS.Models;
using UsefulCore.Enums.Roles;
using UsefulDatabase.Model;

namespace UsefulCMS.Pages
{
    public class IndexModel : AuthorizedCMSPageModel
    {
        private readonly UsefulContext _context;

        public int UserCount { get; set; }
        public int SuperAdminCount { get; set; }
        public int AdminCount { get; set; }
        public int StandardCount { get; set; }

        public IndexModel(IMapper mapper, UsefulContext context) : base(mapper)
        {
            _context = context;
        }

        public async Task OnGetAsync()
        {
            SuperAdminCount = await CountForRole(RoleType.SuperAdministrator.ToString());
            AdminCount = await CountForRole(RoleType.Administrator.ToString());
            StandardCount = await CountForRole(RoleType.Standard.ToString());
            UserCount = await _context.Users.CountAsync();

            async Task<int> CountForRole(string role)
            {
                return await _context.Users.Where(u => u.Roles.Any(r => r.Role.Name == role)).CountAsync();
            }
        }
    }
}
