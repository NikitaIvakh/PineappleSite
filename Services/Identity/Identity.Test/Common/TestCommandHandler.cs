using Moq;
using AutoMapper;
using Identity.Application.Mapping;
using Identity.Application.Services;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Identity.Application.Validators;
using Identity.Domain.Entities.Users;
using Identity.Domain.Interfaces;
using Identity.Infrastructure.Repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;

namespace Identity.Test.Common;

public class TestCommandHandler : IDisposable
{
    protected readonly DeleteUserValidator DeleteUser;
    protected readonly DeleteUsersValidator DeleteUsers;
    protected readonly AuthRequestValidator AuthRequest;

    protected readonly IMapper Mapper;
    protected readonly ApplicationDbContext Context;
    protected readonly IHttpContextAccessor HttpContextAccessor;
    protected readonly IMemoryCache MemoryCache;
    protected readonly IUserRepository UserRepository;
    protected readonly ITokenService TokenService;
    protected readonly IConfiguration Configuration;

    protected TestCommandHandler()
    {
        Context = IdentityDbContextFactory.Create();
        var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();

        var configurationObj = new Mock<IConfiguration>();
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var tokenServiceObj = new Mock<ITokenService>();
        Configuration = new ConfigurationManager();

        DeleteUser = new DeleteUserValidator();
        DeleteUsers = new DeleteUsersValidator();
        AuthRequest = new AuthRequestValidator();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());

        TokenService = new TokenService(new ConfigurationManager());

        UserManager<ApplicationUser> userManager = new(
            new UserStore<ApplicationUser>(Context),
            null,
            Mock.Of<IPasswordHasher<ApplicationUser>>(),
            Array.Empty<IUserValidator<ApplicationUser>>(),
            Array.Empty<IPasswordValidator<ApplicationUser>>(),
            Mock.Of<ILookupNormalizer>(),
            Mock.Of<IdentityErrorDescriber>(),
            Mock.Of<IServiceProvider>(),
            Mock.Of<Microsoft.Extensions.Logging.ILogger<UserManager<ApplicationUser>>>());

        UserRepository = new UserRepository(Context, userManager);
    }

    public void Dispose() => IdentityDbContextFactory.Destroy(Context);
}