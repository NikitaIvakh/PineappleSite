//using AutoMapper;
//using ShoppingCart.Application.Profiles;
//using ShoppingCart.Infrastructure;

//namespace ShoppingCart.Test.Common
//{
//    public class TestCommandHandler : IDisposable
//    {
//        protected ApplicationDbContext Context;
//        protected IMapper Mapper;

//        public TestCommandHandler()
//        {
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
//}