//using AutoMapper;
//using Identity.Application.Features.Identities.Commands.Commands;
//using Identity.Application.Profiles;
//using Identity.Application.Validators;
//using Identity.Domain.DTOs.Authentications;
//using Identity.Domain.Entities.Users;
//using Identity.Domain.Interface;
//using Identity.Infrastructure;
//using Microsoft.AspNetCore.Authentication;
//using Microsoft.AspNetCore.Http;
//using Microsoft.AspNetCore.Identity;
//using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using Microsoft.Extensions.Options;
//using Moq;
//using Serilog;

//namespace Identity.Test.Common
//{
//    public class TestCommandHandler : IDisposable
//    {
//        #region Validators
//        protected IDeleteUserListDtoValidator DeleteUsers;
//        protected IDeleteUserDtoValidator DeleteUser;
//        protected IAuthRequestDtoValidator AuthRequest;
//        #endregion

//        #region
//        protected ILogger DeleteUserListLogger;
//        protected ILogger DeleteUserLogger;
//        protected ILogger AuthUserLogger;
//        #endregion

//        protected IMapper Mapper;
//        protected PineAppleIdentityDbContext Context;
//        protected UserManager<ApplicationUser> UserManager;
//        protected SignInManager<ApplicationUser> SignInManager;
//        protected IOptions<JwtSettings> JwtSettings;
//        protected ITokenProvider TokenProvider;
//        protected IHttpContextAccessor HttpContextAccessor;

//        public TestCommandHandler()
//        {
//            var token = new Mock<ITokenProvider>();
//            var httpContextAccessor = new Mock<IHttpContextAccessor>();
//            Context = IdentityDbContextFactory.Create();
//            DeleteUserListLogger = Log.ForContext<DeleteUsersRequestHandler>();
//            DeleteUserLogger = Log.ForContext<DeleteUserRequestHandler>();
//            AuthUserLogger = Log.ForContext<LoginUserRequestHandler>();
//            UserManager = new UserManager<ApplicationUser>(
//                new UserStore<ApplicationUser>(Context),
//                null,
//                Mock.Of<IPasswordHasher<ApplicationUser>>(),
//                Array.Empty<IUserValidator<ApplicationUser>>(),
//                Array.Empty<IPasswordValidator<ApplicationUser>>(),
//                Mock.Of<ILookupNormalizer>(),
//                Mock.Of<IdentityErrorDescriber>(),
//                Mock.Of<IServiceProvider>(),
//                Mock.Of<Microsoft.Extensions.Logging.ILogger<UserManager<ApplicationUser>>>());

//            SignInManager = new(
//                UserManager, 
//                Mock.Of<IHttpContextAccessor>(), 
//                Mock.Of<IUserClaimsPrincipalFactory<ApplicationUser>>(), 
//                null, 
//                Mock.Of<Microsoft.Extensions.Logging.ILogger<SignInManager<ApplicationUser>>>(),
//                Mock.Of<IAuthenticationSchemeProvider>(), 
//                null);

//            var jwtSettings = Mock.Of<JwtSettings>();
//            JwtSettings = Options.Create(jwtSettings);
//            TokenProvider = token.Object;
//            HttpContextAccessor = httpContextAccessor.Object;

//            DeleteUsers = new IDeleteUserListDtoValidator();
//            DeleteUser = new IDeleteUserDtoValidator();
//            AuthRequest = new IAuthRequestDtoValidator(Context);

//            var mapperConfiguration = new MapperConfiguration(config =>
//            {
//                config.AddProfile<MappingProfile>();
//            });

//            Mapper = mapperConfiguration.CreateMapper();
//        }

//        public void Dispose()
//        {
//            IdentityDbContextFactory.Destroy(Context);
//        }
//    }
//}