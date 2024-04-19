using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Services;

public sealed class CouponService(
    ILocalStorageService localStorageService,
    ICouponClient couponClient,
    IMapper mapper,
    IHttpContextAccessor httpContextAccessor)
    : BaseCouponService(localStorageService, couponClient, httpContextAccessor), ICouponService
{
    private readonly ICouponClient _couponClient = couponClient;

    public async Task<CollectionResultViewModel<GetCouponsViewModel>> GetAllCouponsAsync()
    {
        AddBearerToken();
        try
        {
            var coupons = await _couponClient.GetCouponsAsync();
            var getCoupons = coupons.Data
                .Select(coupon => new GetCouponsViewModel
                (
                    coupon.CouponId,
                    coupon.CouponCode,
                    coupon.DiscountAmount,
                    coupon.MinAmount
                )).ToList();

            if (coupons.IsSuccess)
            {
                return new CollectionResultViewModel<GetCouponsViewModel>
                {
                    Count = coupons.Count,
                    StatusCode = coupons.StatusCode,
                    SuccessMessage = coupons.SuccessMessage,
                    Data = getCoupons,
                };
            }

            return new CollectionResultViewModel<GetCouponsViewModel>
            {
                StatusCode = coupons.StatusCode,
                ErrorMessage = coupons.ErrorMessage,
                ValidationErrors = string.Join(", ", coupons.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new CollectionResultViewModel<GetCouponsViewModel>()
            {
                StatusCode = ex.StatusCode,
                ErrorMessage = ex.Response,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }
    
    public async Task<ResultViewModel<GetCouponViewModel>> GetCouponByIdAsync(string couponId)
    {
        AddBearerToken();
        try
        {
            var coupon = await _couponClient.GetCouponByIdAsync(couponId);
            var getCoupon = new GetCouponViewModel(coupon.Data.CouponId, coupon.Data.CouponCode,
                coupon.Data.DiscountAmount, coupon.Data.MinAmount);

            if (coupon.IsSuccess)
            {
                return new ResultViewModel<GetCouponViewModel>
                {
                    Data = getCoupon,
                    StatusCode = coupon.StatusCode,
                    SuccessMessage = coupon.SuccessMessage,
                };
            }

            return new ResultViewModel<GetCouponViewModel>
            {
                StatusCode = coupon.StatusCode,
                ErrorMessage = coupon.ErrorMessage,
                ValidationErrors = string.Join(", ", coupon.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new ResultViewModel<GetCouponViewModel>
            {
                StatusCode = ConvertCouponExceptions(ex).StatusCode,
                ErrorMessage = ConvertCouponExceptions(ex).ErrorMessage,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }
    
    public async Task<ResultViewModel<GetCouponViewModel>> GetCouponByCodeAsync(string couponCode)
    {
        AddBearerToken();
        try
        {
            var coupon = await _couponClient.GetCouponByCodeAsync(couponCode);
            var getCoupon = new GetCouponViewModel(coupon.Data.CouponId, coupon.Data.CouponCode,
                coupon.Data.DiscountAmount, coupon.Data.MinAmount);

            if (coupon.IsSuccess)
            {
                return new ResultViewModel<GetCouponViewModel>
                {
                    Data = getCoupon,
                    StatusCode = coupon.StatusCode,
                    SuccessMessage = coupon.SuccessMessage,
                };
            }

            return new ResultViewModel<GetCouponViewModel>
            {
                StatusCode = coupon.StatusCode,
                ErrorMessage = coupon.ErrorMessage,
                ValidationErrors = string.Join(", ", coupon.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new ResultViewModel<GetCouponViewModel>
            {
                StatusCode = ConvertCouponExceptions(ex).StatusCode,
                ErrorMessage = ConvertCouponExceptions(ex).ErrorMessage,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }

    public async Task<ResultViewModel<string>> CreateCouponAsync(CreateCouponViewModel createCoupon)
    {
        AddBearerToken();
        try
        {
            var createCouponDto = mapper.Map<CreateCouponDto>(createCoupon);
            var apiResponse = await _couponClient.CreateCouponAsync(createCouponDto);

            if (apiResponse.IsSuccess)
            {
                return new ResultViewModel<string>
                {
                    Data = apiResponse.Data,
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new ResultViewModel<string>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }
        
        catch (CouponExceptions<string> ex)
        {
            return new ResultViewModel<string>
            {
                StatusCode = ConvertCouponExceptions(ex).StatusCode,
                ErrorMessage = ConvertCouponExceptions(ex).ErrorMessage,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }
    
    public async Task<ResultViewModel> UpdateCouponAsync(string couponId, UpdateCouponViewModel updateCoupon)
    {
        AddBearerToken();
        try
        {
            var updateCouponDto = mapper.Map<UpdateCouponDto>(updateCoupon);
            var apiResponse = await _couponClient.UpdateCouponAsync(couponId, updateCouponDto);

            if (apiResponse.IsSuccess)
            {
                return new ResultViewModel
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new ResultViewModel
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new ResultViewModel
            {
                StatusCode = ConvertCouponExceptions(ex).StatusCode,
                ErrorMessage = ConvertCouponExceptions(ex).ErrorMessage,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }
    
    public async Task<ResultViewModel> DeleteCouponAsync(string couponId, DeleteCouponViewModel deleteCoupon)
    {
        AddBearerToken();
        try
        {
            var deleteCouponDto = mapper.Map<DeleteCouponDto>(deleteCoupon);
            var apiResponse = await _couponClient.DeleteCouponAsync(couponId, deleteCouponDto);

            if (apiResponse.IsSuccess)
            {
                return new ResultViewModel
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new ResultViewModel
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new ResultViewModel
            {
                StatusCode = ConvertCouponExceptions(ex).StatusCode,
                ErrorMessage = ConvertCouponExceptions(ex).ErrorMessage,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }

    public async Task<CollectionResultViewModel<bool>> DeleteCouponsAsync(DeleteCouponsViewModel deleteCoupons)
    {
        AddBearerToken();
        try
        {
            var deleteCouponListDto = mapper.Map<DeleteCouponsDto>(deleteCoupons);
            var apiResponse = await _couponClient.DeleteCouponsAsync(deleteCouponListDto);

            if (apiResponse.IsSuccess)
            {
                return new CollectionResultViewModel<bool>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                };
            }

            return new CollectionResultViewModel<bool>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new CollectionResultViewModel<bool>
            {
                StatusCode = ConvertCouponExceptions(ex).StatusCode,
                ErrorMessage = ConvertCouponExceptions(ex).ErrorMessage,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }
}