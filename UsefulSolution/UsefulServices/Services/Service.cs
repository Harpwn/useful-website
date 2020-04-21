using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using UsefulDatabase.Model;

namespace UsefulServices.Services
{
    public abstract class Service
    {
        protected UsefulContext Context;
        protected IMemoryCache Cache;
        protected IMapper Mapper;

        public Service(UsefulContext context, IMemoryCache cache, IMapper mapper)
        {
            Context = context;
            Cache = cache;
            Mapper = mapper;
        }
    }
}
