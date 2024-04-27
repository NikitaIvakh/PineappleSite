using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Order.Application.Mapping;
using Order.Domain.DTOs;
using Order.Domain.Entities;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Repository;
using Order.Domain.Interfaces.Services;
using Order.Domain.ResultOrder;
using Order.Infrastructure;
using Order.Infrastructure.Repository.Implementation;

namespace Orders.Test.Common;

public class TestQueryHandler : IDisposable
{
    protected ApplicationDbContext Context;
    protected IMapper Mapper;
    protected IMemoryCache MemoryCache;
    protected IHttpContextAccessor HttpContextAccessor;
    protected IBaseRepository<OrderHeader> OrderHeader;
    protected IBaseRepository<OrderDetails> OrderDetails;

    protected IProductService ProductService;
    protected IUserService UserService;

    protected TestQueryHandler()
    {
        var httpContextAccessorObjet = new Mock<IHttpContextAccessor>();
        Context = OrdersDbContextFactory.Create();

        var mapperConfiguration = new MapperConfiguration(options => { options.AddProfile<MappingProfile>(); });

        Mapper = mapperConfiguration.CreateMapper();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());
        HttpContextAccessor = httpContextAccessorObjet.Object;
        OrderHeader = new BaseRepository<OrderHeader>(Context);
        OrderDetails = new BaseRepository<OrderDetails>(Context);

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

        var userMock = new Mock<IUserService>();
        userMock.Setup(mock => mock.GetUserAsync("testuser5t654"))
            .ReturnsAsync(new Result<GetUserDto>
            {
                Data = new GetUserDto
                (
                    UserId: "testuser5t654",
                    FirstName: "firstname",
                    LastName: "lastname",
                    EmailAddress: "emailaddress@gmail.com",
                    UserName: "usernamswswqdweq",
                    CreatedTime: DateTime.Today,
                    ModifiedTime: DateTime.Today,
                    Role: new List<string>() { "User" }
                ),
            });

        ProductService = productMock.Object;
        UserService = userMock.Object;
    }

    public void Dispose() => OrdersDbContextFactory.Destroy(Context);
}