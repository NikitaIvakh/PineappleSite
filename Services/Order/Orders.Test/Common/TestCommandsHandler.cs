using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Order.Application.Mapping;
using Order.Domain.Entities;
using Order.Domain.Interfaces.Repository;
using Order.Infrastructure.Repository.Implementation;
using Order.Infrastructure;
using Order.Application.Validators;
using Order.Domain.DTOs;
using Order.Domain.Enum;
using Order.Domain.Interfaces.Services;
using Order.Domain.ResultOrder;

namespace Orders.Test.Common;

public class TestCommandsHandler : IDisposable
{
    protected ApplicationDbContext Context;
    protected IMapper Mapper;
    protected IMemoryCache MemoryCache;
    protected IBaseRepository<OrderHeader> OrderHeader;
    protected IBaseRepository<OrderDetails> OrderDetails;
    protected OrderValidator CreateValidator;

    protected IProductService ProductService;
    protected IUserService UserService;

    public TestCommandsHandler()
    {
        Context = OrdersDbContextFactory.Create();

        var mapperConfiguration = new MapperConfiguration(options => { options.AddProfile<MappingProfile>(); });

        Mapper = mapperConfiguration.CreateMapper();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());
        OrderHeader = new BaseRepository<OrderHeader>(Context);
        OrderDetails = new BaseRepository<OrderDetails>(Context);
        CreateValidator = new OrderValidator();

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

    public void Dispose()
    {
        OrdersDbContextFactory.Destroy(Context);
    }
}