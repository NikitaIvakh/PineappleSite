using Carter;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Coupon.API.Endpoints;

public class CouponEndpoints : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/coupons");

        group.MapGet("", GetCoupons).WithName(nameof(GetCoupons));
    }

    private static async Task<Results<Ok<CollectionResult<GetCouponsDto>>, BadRequest<string>>> GetCoupons(
        ISender sender)
    {
        var request = await sender.Send(new GetCouponsRequest());

        if (request.IsSuccess)
        {
            return TypedResults.Ok(request);
        }

        return TypedResults.BadRequest(string.Join(", ", request.ValidationErrors!));
    }
}