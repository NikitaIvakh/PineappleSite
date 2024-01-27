using AutoMapper;
using Coupon.Infrastructure;
using Coupon.Application.Mapping;
using Xunit;
using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Serilog;
using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Validations;
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
            var repositoryMock = new Mock<IBaseRepository<CouponEntity>>();

            Logger = Log.ForContext<CreateCouponRequestHandler>(); ;
            Repository = repositoryMock.Object;

            Context = CouponRepositoryContextFactory.Create(Repository);

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