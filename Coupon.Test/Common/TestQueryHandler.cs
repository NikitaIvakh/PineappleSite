using AutoMapper;
using Coupon.Application.Profiles;
using Coupon.Infrastructure;
using PineappleSite.Coupon.Core.Interfaces;
using Xunit;

namespace Coupon.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        private readonly ApplicationDbContext _context;
        public IMapper Mapper;

        public TestQueryHandler()
        {
            _context = CouponRepositoryContextFactory.Create();

            var configuration = new MapperConfiguration(key =>
            {
                key.AddProfile<MappingProfile>();
            });

            Mapper = configuration.CreateMapper();
        }

        public void Dispose()
        {
            CouponRepositoryContextFactory.DestroyDatabase(_context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}