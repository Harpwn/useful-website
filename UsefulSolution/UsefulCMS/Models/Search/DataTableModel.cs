using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UsefulCMS.Search;
using UsefulDatabase.Model;

namespace UsefulCMS.Models.Search
{
    public abstract class DataTableModel<TEntity, TPageSearch> : AuthorizedCMSPageModel
        where TPageSearch : PageSearch
        where TEntity : class
    {
        protected readonly UsefulContext Context;

        public string CurrentFilter { get; set; }
        public string CurrentSort { get; set; }
        public PaginatedList<TEntity> ItemList { get; set; }

        public DataTableModel(UsefulContext context, IMapper mapper) : base(mapper)
        {
            Context = context;
        }

        public async Task OnGetAsync([FromQuery]TPageSearch data)
        {
            CurrentSort = data.SortOrder;
            SetupSort(data);

            if (data.SearchString != null)
            {
                data.PageIndex = 1;
            }
            else
            {
                data.SearchString = data.CurrentFilter;
            }

            CurrentFilter = data.SearchString;

            IQueryable<TEntity> items = GetEntities(data);
            items = FilterEntities(items, data);
            items = OrderEntities(items, data.SortOrder);

            ItemList = await PaginatedList<TEntity>.CreateAsync(
                items.AsNoTracking(), data.PageIndex, data.PageSize);
        }

        protected abstract void SetupSort(TPageSearch data);

        protected abstract IQueryable<TEntity> GetEntities(TPageSearch data);

        protected abstract IQueryable<TEntity> FilterEntities(IQueryable<TEntity> items, TPageSearch data);

        protected abstract IQueryable<TEntity> OrderEntities(IQueryable<TEntity> items, string sortOrder);

    }
}
