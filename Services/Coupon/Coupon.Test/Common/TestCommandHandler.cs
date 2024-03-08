using AutoMapper;
using Coupon.Application.Features.Coupons.Handlers.Commands;
using Coupon.Application.Mapping;
using Coupon.Application.Validations;
using Coupon.Domain.Entities;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Infrastructure;
using Coupon.Infrastructure.Repository;
using Moq;
using Serilog;

namespace Coupon.Test.Common
{
    public class TestCommandHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected CreateValidator CreateValidator;
        protected UpdateValidator UpdateValidator;
        protected DeleteValidator DeleteValidator;
        protected DeleteListValidator DeleteListValidator;
        protected IBaseRepository<CouponEntity> Repository;
        protected ILogger CreateLogger;
        protected ILogger UpdateLogger;
        protected ILogger DeleteLogger;
        protected ILogger DeleteListLogger;
        protected IMapper Mapper;

        public TestCommandHandler()
        {
            Context = CouponRepositoryContextFactory.Create();
            Repository = new BaseRepository<CouponEntity>(Context);
            CreateLogger = Log.ForContext<CreateCouponRequestHandler>();
            UpdateLogger = Log.ForContext<UpdateCouponRequestHandler>();
            DeleteLogger = Log.ForContext<DeleteCouponRequestHandler>();
            DeleteListLogger = Log.ForContext<DeleteCouponListRequestHandler>();

            CreateValidator = new CreateValidator(Context);
            UpdateValidator = new UpdateValidator(Context);
            DeleteValidator = new DeleteValidator(Context);
            DeleteListValidator = new DeleteListValidator();

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
}