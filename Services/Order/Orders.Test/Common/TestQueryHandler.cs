﻿using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Order.Application.Mapping;
using Order.Domain.Entities;
using Order.Domain.Interfaces.Repository;
using Order.Domain.Interfaces.Services;
using Order.Infrastructure;
using Order.Infrastructure.Repository.Implementation;
using Order.Infrastructure.Repository.Services;

namespace Orders.Test.Common
{
    [CollectionDefinition("QueryCollection")]
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected IMemoryCache MemoryCache;
        protected IUserService UserService;
        protected IHttpContextAccessor HttpContextAccessor;
        protected IBaseRepository<OrderHeader> OrderHeader;
        protected IBaseRepository<OrderDetails> OrderDetails;

        public TestQueryHandler()
        {
            var httpContextAccessorObjet = new Mock<IHttpContextAccessor>();
            Context = OrdersDbContextFactory.Create();

            var mapperConfiguration = new MapperConfiguration(options =>
            {
                options.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
            MemoryCache = new MemoryCache(new MemoryCacheOptions());
            HttpContextAccessor = httpContextAccessorObjet.Object;
            OrderHeader = new BaseRepository<OrderHeader>(Context);
            OrderDetails = new BaseRepository<OrderDetails>(Context);

            var userServiceObject = new Mock<IUserService>();

            userServiceObject.Setup(mock => mock.GetUserAsync("tetsuserid"))
                .ReturnsAsync(new Order.Domain.ResultOrder.Result<Order.Domain.DTOs.UserWithRolesDto>
                {
                    Data = new Order.Domain.DTOs.UserWithRolesDto
                    {
                        User = new Order.Domain.DTOs.ApplicationUserDto
                        {
                            Id = "test1",
                            FirstName = "test1",
                            LastName = "test1",
                            Age = 25,
                            Description = "test1test1test1test1",
                            RefreshToken = "test1test1test1test1refreshToken",
                            RefreshTokenExpiresTime = DateTime.UtcNow.AddDays(6),
                            ImageUrl = "ImageUrl",
                            ImageLocalPath = "ImageLocalPath"
                        },

                        Roles = ["Administrator"]
                    }
                });

            UserService = userServiceObject.Object;
        }

        public void Dispose()
        {
            OrdersDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}