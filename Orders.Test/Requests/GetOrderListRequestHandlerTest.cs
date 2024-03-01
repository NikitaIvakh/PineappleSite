using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Order.Application.Features.Handlers.Requests;
using Order.Application.Utility;
using Orders.Test.Common;
using System.Security.Claims;

namespace Orders.Test.Requests
{
    public class GetOrderListRequestHandlerTest : TestQueryHandler
    {
        [Fact]
        public async Task GetOrderListRequestHandlerTest_Fail()
        {
            // Arrange
            var handler = new GetOrderListRequestHandler(OrderHeader, HttpContextAccessor, Mapper, MemoryCache);
            string userId = "8e445865-a24d-4543-a6c6-9443d048cdb9";

            // Mock HttpContext
            var claims = new List<Claim>
            {
                new(ClaimTypes.Role, StaticDetails.RoleAdmin)
            };

            var identity = new ClaimsIdentity(claims, "TestAuthentication");
            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, userId));
            identity.AddClaim(new Claim(ClaimTypes.Name, "TestUser"));
            var principal = new ClaimsPrincipal(identity);

            var context = new DefaultHttpContext
            {
                User = principal,
                RequestServices = new ServiceCollection().BuildServiceProvider()
            };

            var httpContextAccessor = new HttpContextAccessor();
            httpContextAccessor.HttpContext = context;

            // Act
            var result = await handler.Handle(new Order.Application.Features.Requests.Requests.GetOrderListRequest
            {
                UserId = userId,
            }, CancellationToken.None);

            // Assert
            result.IsSuccess.Should().BeFalse();
            result.ErrorMessage.Should().Be("Внутренняя проблема сервера");
            result.SuccessMessage.Should().BeNullOrEmpty();
        }
    }
}