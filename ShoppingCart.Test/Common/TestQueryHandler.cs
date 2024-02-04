//using AutoMapper;
//using Moq;
//using ShoppingCart.Application.Profiles;
//using ShoppingCart.Domain.Interfaces;
//using ShoppingCart.Infrastructure;
//using Xunit;

//namespace ShoppingCart.Test.Common
//{
//    public class TestQueryHandler : IDisposable
//    {
//        protected ApplicationDbContext Context;
//        protected IMapper Mapper;
//        protected IProductService ProductService;
//        protected ICouponService CouponService;

//        public TestQueryHandler()
//        {
//            var productServiceMock = new Mock<IProductService>();
//            var couponServiceMock = new Mock<ICouponService>();

//            ProductService = productServiceMock.Object;
//            CouponService = couponServiceMock.Object;

//            Context = ShoppingCartDbContextFactory.Create();

//            var mapperConfiguration = new MapperConfiguration(config =>
//            {
//                config.AddProfile<MappingProfile>();
//            });

//            Mapper = mapperConfiguration.CreateMapper();
//        }

//        public void Dispose()
//        {
//            ShoppingCartDbContextFactory.Desctroy(Context);
//        }
//    }

//    [CollectionDefinition("QueryCollection")]
//    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
//}