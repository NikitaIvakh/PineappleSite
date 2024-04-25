using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Memory;
using Moq;
using Product.Application.Features.Commands.Handlers;
using Product.Application.Mapping;
using Product.Application.Validations;
using Product.Domain.Interfaces;
using Product.Infrastructure;
using Product.Infrastructure.Repository;
using Serilog;

namespace Product.Test.Common;

public class TestCommandHandler : IDisposable
{
    protected ApplicationDbContext Context;
    protected IMapper Mapper;
    protected IProductRepository Repository;
    protected ILogger CreateLogger;
    protected ILogger UpdateLogger;
    protected ILogger DeleteLogger;
    protected ILogger DeleteListLogger;
    protected IMemoryCache MemoryCache;

    protected CreateValidator CreateValidator;
    protected UpdateValidator UpdateValidator;
    protected DeleteValidator DeleteValidator;
    protected DeleteProductsValidator DeleteProductsValidator;
    protected IHttpContextAccessor HttpContextAccessor;

    protected TestCommandHandler()
    {
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        HttpContextAccessor = httpContextAccessor.Object;

        Context = ProductDbContextFactory.Create();
        Repository = new ProductRepository(Context);
        CreateLogger = Log.ForContext<CreateProductRequestHandler>();
        UpdateLogger = Log.ForContext<UpdateProductRequestHandler>();
        DeleteLogger = Log.ForContext<DeleteProductRequestHandler>();
        DeleteListLogger = Log.ForContext<DeleteProductsRequestHandler>();

        CreateValidator = new CreateValidator();
        UpdateValidator = new UpdateValidator();
        DeleteValidator = new DeleteValidator();
        DeleteProductsValidator = new DeleteProductsValidator();

        MemoryCache = new MemoryCache(new MemoryCacheOptions());
        var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    public void Dispose() => ProductDbContextFactory.Destroy(Context);
}