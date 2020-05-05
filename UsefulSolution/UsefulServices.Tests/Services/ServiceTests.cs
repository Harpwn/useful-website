using AutoMapper;

namespace UsefulServices.Tests.Services
{
    public abstract class ServiceTests
    {
        internal IMapper mapper;

        public ServiceTests()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddMaps(new[] {
                    "UsefulServices",
                })
            );
            mapper = new Mapper(configuration);
        }
    }
}
