using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Product.Application.Mapping;
using Product.Domain.Interfaces;
using Product.Infrastructure;
using Product.Infrastructure.Repository;

namespace Product.Test.Common;

public class TestQueryHandler : IDisposable
{
    protected ApplicationDbContext Context;
    protected IMapper Mapper;
    protected IProductRepository Repository;
    protected IMemoryCache MemoryCache;

    protected TestQueryHandler()
    {
        Context = ProductDbContextFactory.Create();
        Repository = new ProductRepository(Context);

        var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });

        Mapper = mapperConfiguration.CreateMapper();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());
    }

    public void Dispose()
    {
        ProductDbContextFactory.Destroy(Context);
    }
}