// using AutoMapper;
// using Microsoft.AspNetCore.Http;
// using Microsoft.Extensions.Caching.Memory;
// using Moq;
// using Product.Application.DTOs.Validator;
// using Product.Application.Features.Commands.Handlers;
// using Product.Application.Mapping;
// using Product.Application.Validations;
// using Product.Domain.Entities.Producrs;
// using Product.Domain.Interfaces;
// using Product.Infrastructure;
// using Product.Infrastructure.Repository;
// using Serilog;
//
// namespace Product.Test.Common
// {
//     public class TestCommandHandler : IDisposable
//     {
//         #region Mail Entities
//         protected ApplicationDbContext Context;
//         protected IMapper Mapper;
//         protected IProductRepository<ProductEntity> Repository;
//         protected ILogger CreateLogger;
//         protected ILogger UpdateLogger;
//         protected ILogger DeleteLogger;
//         protected ILogger DeleteListLogger;
//         protected IMemoryCache MemoryCache;
//         #endregion
//
//         #region ValidEntities
//         protected CreateValidator CreateValidator;
//         protected UpdateValidator UpdateValidator;
//         protected DeleteValidator DeleteValidator;
//         protected DeleteProductsValidator DeleteProductsValidator;
//         protected IHttpContextAccessor HttpContextAccessor;
//         #endregion
//
//         public TestCommandHandler()
//         {
//             var httpContextAccessor = new Mock<IHttpContextAccessor>();
//             HttpContextAccessor = httpContextAccessor.Object;
//
//             Context = ProductDbContextFactory.Create();
//             Repository = new ProductRepository<ProductEntity>(Context);
//             CreateLogger = Log.ForContext<CreateProductRequestHandler>();
//             UpdateLogger = Log.ForContext<UpdateProductRequestHandler>();
//             DeleteLogger = Log.ForContext<DeleteProductRequestHandler>();
//             DeleteListLogger = Log.ForContext<DeleteProductsRequestHandler>();
//
//             CreateValidator = new CreateValidator();
//             UpdateValidator = new UpdateValidator();
//             DeleteValidator = new DeleteValidator();
//             DeleteProductsValidator = new DeleteProductsValidator();
//
//             MemoryCache = new MemoryCache(new MemoryCacheOptions());
//
//             var mapperConfiguration = new MapperConfiguration(config =>
//             {
//                 config.AddProfile<MappingProfile>();
//             });
//
//             Mapper = mapperConfiguration.CreateMapper();
//         }
//
//         public void Dispose()
//         {
//             ProductDbContextFactory.Destroy(Context);
//         }
//     }
// }