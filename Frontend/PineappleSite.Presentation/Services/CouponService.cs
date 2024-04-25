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

    public async Task<CollectionResultViewModel<CouponViewModel>> GetAllCouponsAsync()
    {
        AddBearerToken();
        try
        {
            var coupons = await _couponClient.GetCouponsAsync();

            if (coupons.IsSuccess)
            {
                return mapper.Map<CollectionResultViewModel<CouponViewModel>>(coupons);
            }

            return new CollectionResultViewModel<CouponViewModel>
            {
                StatusCode = coupons.StatusCode,
                ErrorMessage = coupons.ErrorMessage,
                ValidationErrors = string.Join(", ", coupons.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new CollectionResultViewModel<CouponViewModel>()
            {
                StatusCode = ex.StatusCode,
                ErrorMessage = ex.Response,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }

    public async Task<ResultViewModel<CouponViewModel>> GetCouponByIdAsync(string couponId)
    {
        AddBearerToken();
        try
        {
            var coupon = await _couponClient.GetCouponByIdAsync(couponId);


            if (coupon.IsSuccess)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    StatusCode = coupon.StatusCode,
                    SuccessMessage = coupon.SuccessMessage,
                    Data = mapper.Map<CouponViewModel>(coupon.Data),
                };
            }

            return new ResultViewModel<CouponViewModel>
            {
                StatusCode = coupon.StatusCode,
                ErrorMessage = coupon.ErrorMessage,
                ValidationErrors = string.Join(", ", coupon.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new ResultViewModel<CouponViewModel>
            {
                StatusCode = ConvertCouponExceptions(ex).StatusCode,
                ErrorMessage = ConvertCouponExceptions(ex).ErrorMessage,
                ValidationErrors = ConvertCouponExceptions(ex).ValidationErrors,
            };
        }
    }

    public async Task<ResultViewModel<CouponViewModel>> GetCouponByCodeAsync(string couponCode)
    {
        AddBearerToken();
        try
        {
            var coupon = await _couponClient.GetCouponByCodeAsync(couponCode);

            if (coupon.IsSuccess)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    StatusCode = coupon.StatusCode,
                    SuccessMessage = coupon.SuccessMessage,
                    Data = mapper.Map<CouponViewModel>(coupon.Data),
                };
            }

            return new ResultViewModel<CouponViewModel>
            {
                StatusCode = coupon.StatusCode,
                ErrorMessage = coupon.ErrorMessage,
                ValidationErrors = string.Join(", ", coupon.ValidationErrors),
            };
        }

        catch (CouponExceptions<string> ex)
        {
            return new ResultViewModel<CouponViewModel>
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