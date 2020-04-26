using AutoMapper;

namespace UsefulCMS.Tests.Pages
{
    public abstract class CMSModelTest
    {
        internal IMapper mapper;

        public CMSModelTest()
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
