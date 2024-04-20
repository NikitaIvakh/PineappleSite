using AutoMapper;
using Favourite.Domain.Entities;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Domain.Results;
using Favourite.Domain.DTOs;
using Favourite.Infrastructure.Repository.Implement;
using Favourite.Infrastructure;
using Moq;
using Favourite.Application.Mapping;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Test.Common;

public class TestQueryHandler : IDisposable
{
    private readonly ApplicationDbContext _context;
    protected readonly IMapper Mapper;
    protected readonly IMemoryCache MemoryCache;

    protected readonly IBaseRepository<FavouriteHeader> FavouriteHeader;
    protected readonly IBaseRepository<FavouriteDetails> FavouriteDetails;
    protected readonly IProductService ProductService;

    protected TestQueryHandler()
    {
        var productMock = new Mock<IProductService>();

        productMock.Setup(mock => mock.GetProductListAsync())
            .ReturnsAsync(new CollectionResult<ProductDto>
            {
                Data = new List<ProductDto>()
                {
                    new() { Id = 4, Name = "Product 1", Description = "Description 1", Price = 10.0 },
                    new() { Id = 5, Name = "Product 2", Description = "Description 2", Price = 20.0 },
                }
            });

        _context = FavouriteProductsDbContextFactory.Create();
        FavouriteHeader = new BaseRepository<FavouriteHeader>(_context);
        FavouriteDetails = new BaseRepository<FavouriteDetails>(_context);
        ProductService = productMock.Object;

        MemoryCache = new MemoryCache(new MemoryCacheOptions());

        var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });

        Mapper = mapperConfiguration.CreateMapper();
    }

    public void Dispose() => FavouriteProductsDbContextFactory.Destroy(_context);
}