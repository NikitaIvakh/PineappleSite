using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Product.Application.DTOs.Validator;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Profiles;
using Product.Domain.Entities.Producrs;
using Product.Domain.Interfaces;
using Product.Infrastructure;
using Product.Infrastructure.Repository;
using Serilog;

namespace Product.Test.Common
{
    public class TestCommandHandler : IDisposable
    {
        #region Mail Entities
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected IBaseRepository<ProductEntity> Repository;
        protected ILogger CreateLogger;
        protected ILogger UpdateLogger;
        protected ILogger DeleteLogger;
        protected ILogger DeleteListLogger;
        protected IMemoryCache MemoryCache;
        #endregion

        #region ValidEntities
        protected ICreateProductDtoValidator CreateValidator;
        protected IUpdateProductDtoValidator UpdateValidator;
        protected IDeleteProductDtoValidator DeleteValidator;
        protected IDeleteProductsDtoValidator DeleteProductsValidator;
        protected IHttpContextAccessor HttpContextAccessor;
        #endregion

        public TestCommandHandler()
        {
            var httpContextAccessor = new Mock<IHttpContextAccessor>();
            HttpContextAccessor = httpContextAccessor.Object;

            Context = ProductDbContextFactory.Create();
            Repository = new BaseRepository<ProductEntity>(Context);
            CreateLogger = Log.ForContext<CreateProductDtoRequestHandler>();
            UpdateLogger = Log.ForContext<UpdateProductDtoRequestHandler>();
            DeleteLogger = Log.ForContext<DeleteProductDtoRequestHandler>();
            DeleteListLogger = Log.ForContext<DeleteProductsDtoRequestHandler>();

            CreateValidator = new ICreateProductDtoValidator();
            UpdateValidator = new IUpdateProductDtoValidator();
            DeleteValidator = new IDeleteProductDtoValidator();
            DeleteProductsValidator = new IDeleteProductsDtoValidator();

            MemoryCache = new MemoryCache(new MemoryCacheOptions());

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