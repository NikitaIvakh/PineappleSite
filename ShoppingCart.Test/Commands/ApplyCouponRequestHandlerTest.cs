//using ShoppingCart.Application.DTOs.Cart;
//using ShoppingCart.Application.Features.Commands.Handlers;
//using ShoppingCart.Application.Features.Requests.Handlers;
//using ShoppingCart.Application.Response;
//using ShoppingCart.Test.Common;
//using Shouldly;
//using Xunit;

//namespace ShoppingCart.Test.Commands
//{
//    public class ApplyCouponRequestHandlerTest : TestCommandHandler
//    {
//        [Fact]
//        public async Task ApplyCouponRequestHandlerTest_Sucess()
//        {
//            // Arrange
//            var handler = new ApplyCouponRequestHandler(Context, Mapper);
//            var cartDto = new CartDto
//            {
//                CartHeader = new CartHeaderDto
//                {
//                    Id = 2,
//                    UserId = "8e445865-a24d-4543-a6c6-9443d048cdb9",
//                    CouponCode = "10OFF",
//                    Discount = 10,
//                    CartTotal = 45,
//                    Name = "Test",
//                    Phone = "Test",
//                    Email = "Test",
//                },

//                CartDetails = new List<CartDetailsDto>()
//                {
//                    new()
//                    {
//                        Id = 2,
//                        CartHeaderId = 2,
//                        ProductId = 2,
//                        Count = 2,
//                    },
//                }
//            };

//            // Act
//            var result = await handler.Handle(new ApplyCouponRequest
//            {
//                CartDto = cartDto,
//            }, CancellationToken.None);

//            // Assert
//            result.IsSuccess.ShouldBeTrue();
//            result.ShouldBeOfType<ShoppingCartAPIResponse>();
//        }
//    }
//}