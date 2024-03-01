using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Order.Application.Mapping;
using Order.Infrastructure;

namespace Orders.Test.Common
{
    [CollectionDefinition("QueryCollection")]
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected IMemoryCache MemoryCache;
        protected IHttpContextAccessor HttpContextAccessor;

        public TestQueryHandler()
        {
            var httpContextAccessorObjet = new Mock<IHttpContextAccessor>();
            Context = OrdersDbContextFactory.Create();

            var mapperConfiguration = new MapperConfiguration(options =>
            {
                options.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
            HttpContextAccessor = httpContextAccessorObjet.Object;
        }

        public void Dispose()
        {
            OrdersDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}