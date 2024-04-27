using AutoMapper;
using Favourite.Domain.DTOs;
using Favourite.Domain.Results;
using Favourite.Domain.Entities;
using Favourite.Domain.Interfaces.Repository;
using Favourite.Domain.Interfaces.Services;
using Favourite.Infrastructure;
using Favourite.Application.Mapping;
using Favourite.Application.Validators;
using Moq;
using Favourite.Infrastructure.Repository.Implement;
using Microsoft.Extensions.Caching.Memory;

namespace Favourite.Test.Common;

public class TestCommandHandler : IDisposable
{
    protected readonly ApplicationDbContext Context;
    protected readonly IMapper Mapper;

    protected readonly IBaseRepository<FavouriteHeader> FavouriteHeader;
    protected readonly IBaseRepository<FavouriteDetails> FavouriteDetails;
    protected readonly DeleteValidator DeleteValidator;
    protected readonly DeleteByUserValidator DeleteByUserValidator;
    protected readonly DeleteProductsValidator DeleteProductsValidator;
    protected readonly IMemoryCache MemoryCache;

    protected TestCommandHandler()
    {
        var productMock = new Mock<IProductService>();

        productMock.Setup(mock => mock.GetProductsAsync())
            .ReturnsAsync(new CollectionResult<ProductDto>
            {
                Data = new List<ProductDto>()
                {
                    new() { Id = 4, Name = "Product 1", Description = "Description 1", Price = 10.0 },
                    new() { Id = 5, Name = "Product 2", Description = "Description 2", Price = 20.0 },
                }
            });

        Context = FavouriteProductsDbContextFactory.Create();
        FavouriteHeader = new BaseRepository<FavouriteHeader>(Context);
        FavouriteDetails = new BaseRepository<FavouriteDetails>(Context);

        MemoryCache = new MemoryCache(new MemoryCacheOptions());

        var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });

        Mapper = mapperConfiguration.CreateMapper();
        DeleteValidator = new DeleteValidator();
        DeleteByUserValidator = new DeleteByUserValidator();
        DeleteProductsValidator = new DeleteProductsValidator();
    }

    public void Dispose() => FavouriteProductsDbContextFactory.Destroy(Context);
}