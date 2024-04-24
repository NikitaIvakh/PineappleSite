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
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Identity.Test.Common;

public class TestCommandHandler : IDisposable
{
    protected readonly DeleteUserValidator DeleteUser;
    protected readonly DeleteUsersValidator DeleteUsers;
    protected readonly CreateUserValidation CreateUserValidation;
    protected readonly UpdateUserValidator UpdateUserValidator;
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
        CreateUserValidation = new CreateUserValidation();
        UpdateUserValidator = new UpdateUserValidator();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());

        TokenService = new TokenService(new ConfigurationManager());

        var userStore = new UserStore<ApplicationUser>(Context);
        var passwordHasher = new PasswordHasher<ApplicationUser>();

        UserManager<ApplicationUser> userManager = new UserManager<ApplicationUser>(
            userStore,
            null,
            passwordHasher,
            new List<IUserValidator<ApplicationUser>>(),
            new List<IPasswordValidator<ApplicationUser>>(),
            new UpperInvariantLookupNormalizer(),
            new IdentityErrorDescriber(),
            new ServiceCollection().BuildServiceProvider(),
            new LoggerFactory().CreateLogger<UserManager<ApplicationUser>>());
        
        UserRepository = new UserRepository(Context, userManager);
    }

    public void Dispose() => IdentityDbContextFactory.Destroy(Context);
}