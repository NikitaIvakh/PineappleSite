using AutoMapper;
using Coupon.Infrastructure;
using Coupon.Application.Mapping;
using Xunit;
using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Serilog;
using Coupon.Application.Features.Coupons.Handlers.Commands;
using Moq;

namespace Coupon.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        public ApplicationDbContext Context;
        protected IBaseRepository<CouponEntity> Repository;
        protected ILogger Logger;
        protected IMapper Mapper;

        public TestQueryHandler()
        {
            Context = CouponRepositoryContextFactory.Create();
            Logger = Log.ForContext<CreateCouponRequestHandler>(); ;

            var repositoryMock = new Mock<IBaseRepository<CouponEntity>>();
            Repository = repositoryMock.Object;

            var configurationProvider = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            });

            Mapper = configurationProvider.CreateMapper();
        }

        public void Dispose()
        {
            CouponRepositoryContextFactory.DestroyDatabase(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}