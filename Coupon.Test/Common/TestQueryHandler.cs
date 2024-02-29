﻿using AutoMapper;
using Coupon.Infrastructure;
using Coupon.Application.Mapping;
using Xunit;
using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Serilog;
using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Infrastructure.Repository;
using Coupon.Application.Features.Coupons.Handlers.Queries;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Coupon.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        public ApplicationDbContext Context;
        protected IBaseRepository<CouponEntity> Repository;
        protected ILogger GetByCodeLogger;
        protected ILogger GetLogger;
        protected ILogger GetListLogger;
        protected IMapper Mapper;
        protected IMemoryCache MemoryCache;

        public TestQueryHandler()
        {
            Context = CouponRepositoryContextFactory.Create();
            GetByCodeLogger = Log.ForContext<GetCouponDetailsByCouponNameRequestHandler>();
            GetLogger = Log.ForContext<GetCouponDetailsRequestHandler>();
            GetListLogger = Log.ForContext<GetCouponListRequestHandler>();
            Repository = new BaseRepository<CouponEntity>(Context);

            var memoryCache = new Mock<IMemoryCache>();
            MemoryCache = memoryCache.Object;

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