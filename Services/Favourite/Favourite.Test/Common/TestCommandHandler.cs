using AutoMapper;
using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using Favourite.Domain.Entities;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Infrastructure;
using Favourite.Application.Mapping;
using Moq;
using Favourite.Infrastructure.Repository.Implement;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Test.Common
{
    public class TestCommandHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;

        protected IBaseRepositiry<FavouriteHeader> FavouriteHeader;
        protected IBaseRepositiry<FavouriteDetails> FavouriteDetails;
        protected IProductService ProductService;
        protected IMemoryCache MemoryCache;

        public TestCommandHandler()
        {
            var productMock = new Mock<IProductService>();

            productMock.Setup(mock => mock.GetProductListAsync())
               .ReturnsAsync(new CollectionResult<ProductDto>
               {
                   Data = new List<ProductDto>
                   {
                        new() { Id = 4, Name = "Product 1", Price = 10.0 },
                        new() { Id = 5, Name = "Product 2", Price = 20.0 },
                   }
               });

            Context = FavouriteProductsDbContextFactory.Create();
            FavouriteHeader = new BaseRepository<FavouriteHeader>(Context);
            FavouriteDetails = new BaseRepository<FavouriteDetails>(Context);
            ProductService = productMock.Object;

            MemoryCache = new MemoryCache(new MemoryCacheOptions());

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            FavouriteProductsDbContextFactory.Destroy(Context);
        }
    }
}