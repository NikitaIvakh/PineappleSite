using AutoMapper;
using Moq;
using ShoppingCart.Application.Mapping;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Infrastructure;
using ShoppingCart.Infrastructure.Repository.Implement;
using Xunit;

namespace ShoppingCart.Test.Common
{
    public class TestQueryHandler
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;

        protected IBaseRepository<CartHeader> CartHeader;
        protected IBaseRepository<CartDetails> CartDetails;
        protected IProductService ProductService;
        protected ICouponService CouponService;

        public TestQueryHandler()
        {
            var productObject = new Mock<IProductService>();
            var couponObject = new Mock<ICouponService>();

            Context = ShoppingCartDbContextFactory.Create();
            CartHeader = new BaseRepository<CartHeader>(Context);
            CartDetails = new BaseRepository<CartDetails>(Context);
            ProductService = productObject.Object;
            CouponService = couponObject.Object;

            var mapperComfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperComfiguration.CreateMapper();
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}