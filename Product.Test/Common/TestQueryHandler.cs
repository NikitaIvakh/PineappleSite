using AutoMapper;
using Product.Application.Features.Commands.Queries;
using Product.Application.Profiles;
using Product.Domain.Entities.Producrs;
using Product.Domain.Interfaces;
using Product.Infrastructure;
using Product.Infrastructure.Repository;
using Serilog;
using Xunit;

namespace Product.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected IBaseRepository<ProductEntity> Repository;
        protected ILogger Logger;

        public TestQueryHandler()
        {
            Context = ProductDbContextFactory.Create();
            Repository = new BaseRepository<ProductEntity>(Context);
            Logger = Log.ForContext<GetProductListRequestHandler>();

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

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}