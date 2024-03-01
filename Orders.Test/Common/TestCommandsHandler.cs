using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Order.Application.Mapping;
using Order.Domain.Entities;
using Order.Domain.Interfaces.Repository;
using Order.Infrastructure.Repository.Implementation;
using Order.Infrastructure;

namespace Orders.Test.Common
{
    public class TestCommandsHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected IMemoryCache MemoryCache;
        protected IBaseRepository<OrderHeader> OrderHeader;
        protected IBaseRepository<OrderDetails> OrderDetails;

        public TestCommandsHandler()
        {
            Context = OrdersDbContextFactory.Create();

            var mapperConfiguration = new MapperConfiguration(options =>
            {
                options.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
            OrderHeader = new BaseRepository<OrderHeader>(Context);
            OrderDetails = new BaseRepository<OrderDetails>(Context);
        }

        public void Dispose()
        {
            OrdersDbContextFactory.Destroy(Context);
        }
    }
}