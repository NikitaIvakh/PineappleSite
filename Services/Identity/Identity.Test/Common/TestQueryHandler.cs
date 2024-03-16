using AutoMapper;
using Identity.Application.Features.Users.Commands.Queries;
using Identity.Application.Profiles;
using Identity.Domain.Entities.Users;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using Serilog;
using Xunit;

namespace Identity.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        protected ApplicationDbContext Context;
        protected IMapper Mapper;
        protected ILogger GetUsersLogger;
        protected ILogger GetUserLogger;
        protected IMemoryCache MemoryCache;
        protected UserManager<ApplicationUser> UserManager;

        public TestQueryHandler()
        {
            Context = IdentityDbContextFactory.Create();
            GetUsersLogger = Log.ForContext<GetUserListRequestHandler>();
            GetUserLogger = Log.ForContext<GetUserDetailsRequestHandler>();
            UserManager = new UserManager<ApplicationUser>(
                new UserStore<ApplicationUser>(Context),
                null,
                Mock.Of<IPasswordHasher<ApplicationUser>>(),
                Array.Empty<IUserValidator<ApplicationUser>>(),
                Array.Empty<IPasswordValidator<ApplicationUser>>(),
                Mock.Of<ILookupNormalizer>(),
                Mock.Of<IdentityErrorDescriber>(),
                Mock.Of<IServiceProvider>(),
                Mock.Of<Microsoft.Extensions.Logging.ILogger<UserManager<ApplicationUser>>>());

            MemoryCache = new MemoryCache(new MemoryCacheOptions());

            var mapperConfiguration = new MapperConfiguration(config =>
            {
                config.AddProfile<MappingProfile>();
            });

            Mapper = mapperConfiguration.CreateMapper();
        }

        public void Dispose()
        {
            IdentityDbContextFactory.Destroy(Context);
        }
    }

    [CollectionDefinition("QueryCollection")]
    public class QueryCollection : ICollectionFixture<TestQueryHandler> { }
}