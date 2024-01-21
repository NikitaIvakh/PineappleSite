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
        protected ICreateProductDtoValidator Validator;

        public TestCommandHandler()
        {
            Context = ProductDbContextFactory.Create();
            Validator = new ICreateProductDtoValidator();

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