using AutoMapper;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UsefulCMS.Models;
using UsefulCMS.Models.Search;
using UsefulCore.Enums.Roles;
using UsefulCore.Models.Users;
using UsefulDatabase.Model;
using UsefulDatabase.Model.Roles;

namespace UsefulCMS.Pages.Administration
{
    public class UsersModel : AuthorizedCMSPageModel
    {
        private readonly UsefulContext _context;

        public string UsernameSort { get; set; }
        public string EmailSort { get; set; }

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }

        public PaginatedList<UserRoleSummary> Users { get; set; }

        public UsersModel(UsefulContext context, IMapper mapper) : base(mapper)
        {
            _context = context;
        }

        public async Task OnGetAsync(string sortOrder,string currentFilter, string searchString, int? pageIndex)
        {
            CurrentSort = sortOrder;
            UsernameSort = string.IsNullOrEmpty(sortOrder) ? "username_desc" : "";
            EmailSort = sortOrder == "Email" ? "email_desc" : "Email";
            if (searchString != null)
            {
                pageIndex = 1;
            }
            else
            {
                searchString = currentFilter;
            }

            CurrentFilter = searchString;

            IQueryable<UserRoleSummary> users =
                from u in _context.Users
                select new UserRoleSummary
                {
                    Id = u.Id,
                    Email = u.Email,
                    UserName = u.UserName,
                    Roles = u.Roles.Select(r => r.Role.Type).ToList()
                };

            if (!string.IsNullOrEmpty(searchString))
            {
                users = users.Where(s => s.UserName.Contains(searchString) || s.Email.Contains(searchString));
            }
            switch (sortOrder)
            {
                case "username_desc":
                    users = users.OrderByDescending(s => s.UserName);
                    break;
                case "Email":
                    users = users.OrderBy(s => s.Email);
                    break;
                case "email_desc":
                    users = users.OrderByDescending(s => s.Email);
                    break;
                default:
                    users = users.OrderBy(s => s.UserName);
                    break;
            }

            int pageSize = 3;
            Users = await PaginatedList<UserRoleSummary>.CreateAsync(
                users.AsNoTracking(), pageIndex ?? 1, pageSize);
        }
    }
}