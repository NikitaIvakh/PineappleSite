using AutoMapper;
using Identity.Application.Features.Identities.Commands.Commands;
using Identity.Application.Profiles;
using Identity.Application.Validators;
using Identity.Domain.Entities.Users;
using Identity.Infrastructure;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Moq;
using Serilog;

namespace Identity.Test.Common
{
    public class TestCommandHandler : IDisposable
    {
        #region Validators
        protected IDeleteUserListDtoValidator DeleteUsers;
        protected IDeleteUserDtoValidator DeleteUser;
        #endregion

        #region
        protected ILogger DeleteUserListLogger;
        protected ILogger DeleteUserLogger;
        #endregion

        protected IMapper Mapper;
        protected PineAppleIdentityDbContext Context;
        protected UserManager<ApplicationUser> UserManager;

        public TestCommandHandler()
        {
            Context = IdentityDbContextFactory.Create();
            DeleteUserListLogger = Log.ForContext<DeleteUserListRequestHandler>();
            DeleteUserLogger = Log.ForContext<DeleteUserRequestHandler>();
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

            DeleteUsers = new IDeleteUserListDtoValidator();
            DeleteUser = new IDeleteUserDtoValidator();

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
}