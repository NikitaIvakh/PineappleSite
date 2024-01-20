using AutoMapper;
using Product.Application.Profiles;
using Product.Infrastructure;

namespace Product.Test.Common
{
    public class TestCommandHandler : IDisposable
    {
        protected PineAppleProductsDbContext Context;
        protected IMapper Mapper;

        public TestCommandHandler()
        {
            Context = ProductDbContextFactory.Create();

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            ProductDbContextFactory.Destroy(Context);
        }
    }
}