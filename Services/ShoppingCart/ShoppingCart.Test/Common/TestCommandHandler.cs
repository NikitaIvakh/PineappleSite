using AutoMapper;
using Moq;
using ShoppingCart.Domain.Entities;
using ShoppingCart.Domain.Results;
using ShoppingCart.Domain.DTOs;
using ShoppingCart.Domain.Interfaces.Repository;
using ShoppingCart.Domain.Interfaces.Service;
using ShoppingCart.Application.Mapping;
using ShoppingCart.Infrastructure;
using ShoppingCart.Infrastructure.Repository.Implement;
using Microsoft.Extensions.Caching.Memory;
using ShoppingCart.Application.Validators;
using ShoppingCart.Domain.Enum;

namespace ShoppingCart.Test.Common;

public class TestCommandHandler : IDisposable
{
    protected ApplicationDbContext Context;
    protected IMapper Mapper;

    protected readonly IBaseRepository<CartHeader> CartHeader;
    protected readonly IBaseRepository<CartDetails> CartDetails;
    protected readonly IProductService ProductService;
    protected readonly ICouponService CouponService;
    protected readonly IMemoryCache MemoryCache;
    protected readonly DeleteValidator DeleteValidator;
    protected readonly DeleteProductsValidator DeleteProductsValidator;

    protected TestCommandHandler()
    {
        var productMock = new Mock<IProductService>();

        productMock.Setup(mock => mock.GetProductsAsync())
            .ReturnsAsync(new CollectionResult<ProductDto>
            {
                Data = new List<ProductDto>
                {
                    new()
                    {
                        Id = 1, Name = "Product 1", Price = 10.0, Description = "description 1",
                        ProductCategory = ProductCategory.Drinks, ImageUrl = "", ImageLocalPath = ""
                    },
                    new()
                    {
                        Id = 2, Name = "Product 2", Price = 20.0, Description = "description 2",
                        ProductCategory = ProductCategory.Drinks, ImageUrl = "", ImageLocalPath = ""
                    },
                }
            });

        var couponMock = new Mock<ICouponService>();

        couponMock.Setup(mock => mock.GetCouponAsync("5OFF"))
            .ReturnsAsync(new Result<CouponDto>
            {
                Data = new CouponDto
                {
                    CouponId = Guid.NewGuid().ToString(),
                    CouponCode = "5OFF",
                    DiscountAmount = 20,
                    MinAmount = 40
                }
            });

        Context = ShoppingCartDbContextFactory.Create();
        CartHeader = new BaseRepository<CartHeader>(Context);
        CartDetails = new BaseRepository<CartDetails>(Context);
        ProductService = productMock.Object;
        CouponService = couponMock.Object;

        MemoryCache = new MemoryCache(new MemoryCacheOptions());

        var mapperComfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });

        Mapper = mapperComfiguration.CreateMapper();
        DeleteValidator = new DeleteValidator();
        DeleteProductsValidator = new DeleteProductsValidator();
    }

    public void Dispose() => ShoppingCartDbContextFactory.Destroy(Context);
}