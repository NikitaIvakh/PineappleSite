using AutoMapper;
using Coupon.Application.Profiles;
using Coupon.Infrastructure;
using PineappleSite.Coupon.Core.Interfaces;
using Xunit;

namespace Coupon.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        public ApplicationDbContext Context;
        public IMapper Mapper;

        public TestQueryHandler()
        {
            Context = CouponRepositoryContextFactory.Create();

            var configuration = new MapperConfiguration(key =>
            {
                key.AddProfile<MappingProfile>();
            });

            Mapper = configuration.CreateMapper();
        }

        public void Dispose()
        {
            CouponRepositoryContextFactory.DestroyDatabase(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}