using AutoMapper;
using Order.Application.Mapping;
using Order.Infrastructure;

namespace Orders.Test.Common
{
    [CollectionDefinition("QueryCollection")]
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;

        public TestQueryHandler()
        {
            Context = OrdersDbContextFactory.Create();

            var mapperConfiguration = new MapperConfiguration(options =>
            {
                options.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            OrdersDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}