//using AutoMapper;
//using Product.Application.Profiles;
//using Product.Infrastructure;
//using Xunit;

//namespace Product.Test.Common
//{
//    public class TestQueryHandler : IDisposable
//    {
//        protected ApplicationDbContext Context;
//        protected IMapper Mapper;

//        public TestQueryHandler()
//        {
//            Context = ProductDbContextFactory.Create();

//            var mapperConfiguration = new MapperConfiguration(config =>
//            {
//                config.AddProfile<MappingProfile>();
//            });

//            Mapper = mapperConfiguration.CreateMapper();
//        }

//        public void Dispose()
//        {
//            ProductDbContextFactory.Destroy(Context);
//        }
//    }

//    [CollectionDefinition("QueryCollection")]
//    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
//}