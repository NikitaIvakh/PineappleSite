﻿using AutoMapper;
using Identity.Application.Features.Identities.Commands.Queries;
using Identity.Application.Profiles;
using Identity.Domain.Entities.Users;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Moq;
using Serilog;
using Xunit;

namespace Identity.Test.Common
{
    public class TestQueryHandler : IDisposable
    {
        protected PineAppleIdentityDbContext Context;
        protected IMapper Mapper;
        protected ILogger GetUsersLogger;
        protected UserManager<ApplicationUser> UserManager;

        public TestQueryHandler()
        {
            Context = IdentityDbContextFactory.Create();
            GetUsersLogger = Log.ForContext<GetUserListRequestHandler>();
            UserManager = new UserManager<ApplicationUser>(Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

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