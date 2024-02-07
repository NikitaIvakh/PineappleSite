using AutoMapper;
using Favourite.Domain.Entities;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Domain.Results;
using Favourite.Domain.DTOs;
using Favourite.Infrastructure.Repository.Implement;
using Favourite.Infrastructure;
using Moq;
using Xunit;
using Favourite.Application.Mapping;

namespace Favourite.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;

        protected IBaseRepositiry<FavouriteHeader> FavouriteHeader;
        protected IBaseRepositiry<FavouriteDetails> FavouriteDetails;
        protected IProductService ProductService;

        public TestQueryHandler()
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

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}