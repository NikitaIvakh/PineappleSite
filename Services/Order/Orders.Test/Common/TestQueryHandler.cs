using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Order.Application.Mapping;
using Order.Domain.Entities;
using Order.Domain.Interfaces.Repository;
using Order.Infrastructure;
using Order.Infrastructure.Repository.Implementation;

namespace Orders.Test.Common
{
    [CollectionDefinition("QueryCollection")]
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected IMemoryCache MemoryCache;
        protected IHttpContextAccessor HttpContextAccessor;
        protected IBaseRepository<OrderHeader> OrderHeader;
        protected IBaseRepository<OrderDetails> OrderDetails;

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
            OrderHeader = new BaseRepository<OrderHeader>(Context);
            OrderDetails = new BaseRepository<OrderDetails>(Context);
        }

        public void Dispose()
        {
            OrdersDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}