using AutoMapper;
using Coupon.Application.DTOs;
using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Application.Profiles;
using Coupon.Core.Entities;
using Moq;
using PineappleSite.Coupon.Core.Interfaces;
using Xunit;

namespace Coupon.Test.Queries
{
    [CollectionDefinition("QueryCollection")]
    public class GetCouponListRequestHandlerTest
    {
        private readonly Mock<ICouponRepository> _mockRepository;
        private readonly IMapper _mapper;

        public GetCouponListRequestHandlerTest()
        {
            _mockRepository = new Mock<ICouponRepository>();
            _mapper = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<MappingProfile>();
            }).CreateMapper();
        }

        [Fact]
        public async Task GetCouponListRequestHandlerTest_Success()
        {
            // Arrange
            var coupons = new List<CouponEntity>
            {
                new() 
                {
                    CouponId = 3,
                    CouponCode = "10OFF",
                    DiscountAmount = 10,
                    MinAmount = 20,
                },

                new() 
                {
                    CouponId = 4,
                    CouponCode = "20OFF",
                    DiscountAmount = 20,
                    MinAmount = 30,
                },

                new() 
                {
                    CouponId = 5,
                    CouponCode = "30OFF",
                    DiscountAmount = 30,
                    MinAmount = 40,
                }
            };

            _mockRepository.Setup(repo => repo.GetAllAsync()).ReturnsAsync(coupons);

            var handler = new GetCouponListRequestHandler(_mockRepository.Object, _mapper);

            // Act
            var result = await handler.Handle(new GetCouponListRequest(), CancellationToken.None);

            // Assert
            Assert.NotNull(result);
            Assert.IsAssignableFrom<IEnumerable<CouponDto>>(result);
            Assert.Equal(3, result.Count());
        }
    }
}