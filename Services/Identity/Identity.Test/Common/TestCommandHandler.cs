using Moq;
using AutoMapper;
using Identity.Application.Mapping;
using Identity.Application.Services;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Identity.Application.Validators;
using Identity.Domain.Interfaces;
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
        var httpContextAccessor = new Mock<IHttpContextAccessor>();
        var userRepositoryObj = new Mock<IUserRepository>();
        var tokenServiceObj = new Mock<ITokenService>();
        var configurationObj = new Mock<IConfiguration>();


        DeleteUser = new DeleteUserValidator();
        DeleteUsers = new DeleteUsersValidator();
        AuthRequest = new AuthRequestValidator();
        MemoryCache = new MemoryCache(new MemoryCacheOptions());

        UserRepository = userRepositoryObj.Object;
        TokenService = tokenServiceObj.Object;
        Configuration = configurationObj.Object;

        Context = IdentityDbContextFactory.Create();
        var mapperConfiguration = new MapperConfiguration(config => { config.AddProfile<MappingProfile>(); });
        Mapper = mapperConfiguration.CreateMapper();
    }

    public void Dispose()
    {
        IdentityDbContextFactory.Destroy(Context);
    }
}