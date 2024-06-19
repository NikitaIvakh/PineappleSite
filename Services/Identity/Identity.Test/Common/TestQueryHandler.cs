using AutoMapper;
using Identity.Infrastructure;
using Identity.Application.Mapping;
using Identity.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Moq;

namespace Identity.Test.Common;

public class TestQueryHandler : IDisposable
{
    private readonly ApplicationDbContext _context;
    protected IMapper Mapper;
    protected readonly IMemoryCache MemoryCache;
    protected readonly UserManager<ApplicationUser> UserManager;

    protected TestQueryHandler()
    {
        _context = IdentityDbContextFactory.Create();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());
        var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
        UserManager = new(
            new UserStore<ApplicationUser>(_context),
            null,
            Mock.Of<IPasswordHasher<ApplicationUser>>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<Microsoft.Extensions.Logging.ILogger<UserManager<ApplicationUser>>>());
    }

    public void Dispose() => IdentityDbContextFactory.Destroy(_context);
}