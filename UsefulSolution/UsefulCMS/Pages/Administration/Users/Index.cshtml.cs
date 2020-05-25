using AutoMapper;
using System.Linq;
using UsefulCMS.Models.Search;
using UsefulCMS.Search;
using UsefulCore.Models.Users;
using UsefulDatabase.Model;

namespace UsefulCMS.Pages.Administration.Users
{
    public class IndexModel : DataTableModel<UserRoleSummary, PageSearch>
    {
        public string UsernameSort { get; set; }
        public string EmailSort { get; set; }

        public IndexModel(UsefulContext context, IMapper mapper) : base(context, mapper) { }

        protected override void SetupSort(PageSearch data)
        {
            UsernameSort = string.IsNullOrEmpty(data.SortOrder) ? "username_desc" : "";
            EmailSort = data.SortOrder == "Email" ? "email_desc" : "Email";
        }

        protected override IQueryable<UserRoleSummary> GetEntities(PageSearch data)
            => from u in Context.Users
               select new UserRoleSummary
               {
                   Id = u.Id,
                   Email = u.Email,
                   UserName = u.UserName,
                   Roles = u.Roles.Select(r => r.Role.Type).ToList()
               };

        protected override IQueryable<UserRoleSummary> OrderEntities(IQueryable<UserRoleSummary> items, string sortOrder)
            => sortOrder switch
            {
                "username_desc" => items.OrderByDescending(s => s.UserName),
                "Email" => items.OrderBy(s => s.Email),
                "email_desc" => items.OrderByDescending(s => s.Email),
                _ => items.OrderBy(s => s.UserName)
            };

        protected override IQueryable<UserRoleSummary> FilterEntities(IQueryable<UserRoleSummary> items, PageSearch data)
        {
            if (!string.IsNullOrEmpty(data.SearchString))
            {
                items = items.Where(s => s.UserName.Contains(data.SearchString) || s.Email.Contains(data.SearchString));
            }

            return items;
        }

    }
}