//using AutoMapper;
//using Microsoft.AspNetCore.Http;
//using Moq;
//using Product.Application.DTOs.Validator;
//using Product.Application.Profiles;
//using Product.Infrastructure;

//namespace Product.Test.Common
//{
//    public class TestCommandHandler : IDisposable
//    {
//        protected ApplicationDbContext Context;
//        protected IMapper Mapper;
//        protected ICreateProductDtoValidator CreateValidator;
//        protected IUpdateProductDtoValidator UpdateValidator;
//        protected IDeleteProductDtoValidator DeleteValidator;
//        protected IDeleteProductsDtoValidator DeleteProductsValidator;
//        protected IHttpContextAccessor HttpContextAccessor;

//        public TestCommandHandler()
//        {
//            var httpContextAccessor = new Mock<IHttpContextAccessor>();
//            HttpContextAccessor = httpContextAccessor.Object;

//            Context = ProductDbContextFactory.Create();
//            CreateValidator = new ICreateProductDtoValidator();
//            UpdateValidator = new IUpdateProductDtoValidator();
//            DeleteValidator = new IDeleteProductDtoValidator();
//            DeleteProductsValidator = new IDeleteProductsDtoValidator();

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
//}