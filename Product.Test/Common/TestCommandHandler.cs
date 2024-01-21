using AutoMapper;
using Product.Application.DTOs.Validator;
using Product.Application.Profiles;
using Product.Infrastructure;

namespace Product.Test.Common
{
    public class TestCommandHandler : IDisposable
    {
        protected PineAppleProductsDbContext Context;
        protected IMapper Mapper;
        protected ICreateProductDtoValidator CreateValidator;
        protected IUpdateProductDtoValidator UpdateValidator;

        public TestCommandHandler()
        {
            Context = ProductDbContextFactory.Create();
            CreateValidator = new ICreateProductDtoValidator();
            UpdateValidator = new IUpdateProductDtoValidator();

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            ProductDbContextFactory.Destroy(Context);
        }
    }
}