// using AutoMapper;
// using Microsoft.Extensions.Caching.Memory;
// using Moq;
// using Product.Application.Features.Commands.Queries;
// using Product.Application.Mapping;
// using Product.Domain.Entities.Producrs;
// using Product.Domain.Interfaces;
// using Product.Infrastructure;
// using Product.Infrastructure.Repository;
// using Serilog;
// using Xunit;
//
// namespace Product.Test.Common
// {
//     public class TestQueryHandler : IDisposable
//     {
//         protected ApplicationDbContext Context;
//         protected IMapper Mapper;
//         protected IProductRepository<ProductEntity> Repository;
//         protected ILogger Logger;
//         protected IMemoryCache MemoryCache;
//
//         public TestQueryHandler()
//         {
//             Context = ProductDbContextFactory.Create();
//             Repository = new ProductRepository<ProductEntity>(Context);
//             Logger = Log.ForContext<GetProductsRequestHandler>();
//
//             var mapperConfiguration = new MapperConfiguration(config =>
//             {
//                 config.AddProfile<MappingProfile>();
//             });
//
//             Mapper = mapperConfiguration.CreateMapper();
//             MemoryCache = new MemoryCache(new MemoryCacheOptions());
//         }
//
//         public void Dispose()
//         {
//             ProductDbContextFactory.Destroy(Context);
//         }
//     }
//
//     [CollectionDefinition("QueryCollection")]
//     public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
// }