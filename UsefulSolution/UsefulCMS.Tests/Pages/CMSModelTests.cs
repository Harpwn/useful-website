using AutoMapper;

namespace UsefulCMS.Tests.Pages
{
    public abstract class CMSModelTests
    {
        internal IMapper mapper;

        public CMSModelTests()
        {
            var configuration = new MapperConfiguration(cfg =>
                cfg.AddMaps(new[] {
                    "UsefulCMS",
                })
            );
            mapper = new Mapper(configuration);
        }
    }
}
