//using AutoMapper;
//using Coupon.Application.Mapping;
//using Coupon.Infrastructure;

//namespace Coupon.Test.Common
//{
//    public class TestCommandHandler : IDisposable
//    {
//        protected ApplicationDbContext Context;
//        protected IMapper Mapper;

//        public TestCommandHandler()
//        {
//            Context = CouponRepositoryContextFactory.Create();

//            var configurationProvider = new MapperConfiguration(key =>
//            {
//                key.AddProfile<MappingProfile>();
//            });

//            Mapper = configurationProvider.CreateMapper();
//        }

//        public void Dispose()
//        {
//            CouponRepositoryContextFactory.DestroyDatabase(Context);
//        }
//    }
//}