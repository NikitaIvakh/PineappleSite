using AutoMapper;
using Moq;
using ShoppingCart.Application.Mapping;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Infrastructure;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Results;
using ShoppingCart.Infrastructure.Repository.Implement;
using ShoppingCart.Infrastructure.Repository.Services;
using Xunit;
using Microsoft.Extensions.Caching.Memory;

namespace ShoppingCart.Test.Common
{
    [CollectionDefinition("QueryCollection")]
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;

        protected IBaseRepository<CartHeader> CartHeader;
        protected IBaseRepository<CartDetails> CartDetails;
        protected IProductService ProductService;
        protected ICouponService CouponService;
        protected IMemoryCache MemoryCache;

        public TestQueryHandler()
        {
            var productMock = new Mock<IProductService>();

            productMock.Setup(mock => mock.GetProductListAsync())
                .ReturnsAsync(new CollectionResult<ProductDto>
                {
                    Data = new List<ProductDto>
                    {
                        new() { Id = 1, Name = "Product 1", Price = 10.0 },
                        new() { Id = 2, Name = "Product 2", Price = 20.0 },
                    }
                });

            var couponMock = new Mock<ICouponService>();

            couponMock.Setup(mock => mock.GetCouponAsync("5OFF"))
                .ReturnsAsync(new Result<CouponDto>
                {
                    Data = new CouponDto { CouponId = 1, CouponCode = "5OFF", DiscountAmount = 20, MinAmount = 40 }
                });

            Context = ShoppingCartDbContextFactory.Create();
            CartHeader = new BaseRepository<CartHeader>(Context);
            CartDetails = new BaseRepository<CartDetails>(Context);
            ProductService = productMock.Object;
            CouponService = couponMock.Object;

            MemoryCache = new MemoryCache(new MemoryCacheOptions());

            var mapperComfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperComfiguration.CreateMapper();
        }

        public void Dispose()
        {
            ShoppingCartDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}