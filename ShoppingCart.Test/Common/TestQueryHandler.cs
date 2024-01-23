using AutoMapper;
using ShoppingCart.Application.Profiles;
using ShoppingCart.Infrastructure;
using Xunit;

namespace ShoppingCart.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        protected ShoppingCartDbContext Context;
        protected IMapper Mapper;

        public TestQueryHandler()
        {
            Context = ShoppingCartDbContextFactory.Create();

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            ShoppingCartDbContextFactory.Desctroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}