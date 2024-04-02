using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Services
{
    public class CouponService(ILocalStorageService localStorageService, ICouponClient couponClient, IMapper mapper, IHttpContextAccessor httpContextAccessor) 
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
                    ))
                    .ToList();

                if (coupons.IsSuccess)
                {
                    return new CollectionResultViewModel<GetCouponsViewModel>
                    {
                        Count = coupons.Count,
                        SuccessCode = coupons.StatusCode,
                        SuccessMessage = coupons.SuccessMessage,
                        Data = getCoupons,
                    };
                }

                else
                {
                    foreach (var error in coupons.ValidationErrors)
                    {
                        return new CollectionResultViewModel<GetCouponsViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = coupons.StatusCode,
                            ErrorMessage = coupons.ErrorMessage,
                        };
                    }
                }

                return new CollectionResultViewModel<GetCouponsViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new CollectionResultViewModel<GetCouponsViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new CollectionResultViewModel<GetCouponsViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new CollectionResultViewModel<GetCouponsViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<ResultViewModel<GetCouponViewModel>> GetCouponAsync(int couponId)
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
                        SuccessCode = coupon.StatusCode,
                        SuccessMessage = coupon.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in coupon.ValidationErrors)
                    {
                        return new ResultViewModel<GetCouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = coupon.StatusCode,
                            ErrorMessage = coupon.ErrorMessage,
                        };
                    }
                }

                return new ResultViewModel<GetCouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ResultViewModel<GetCouponViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ResultViewModel<GetCouponViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ResultViewModel<GetCouponViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<ResultViewModel<GetCouponViewModel>> GetCouponAsync(string couponCode)
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
                        SuccessCode = coupon.StatusCode,
                        SuccessMessage = coupon.SuccessMessage,
                    };
                }

                else
                {
                    foreach (var error in coupon.ValidationErrors)
                    {
                        return new ResultViewModel<GetCouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = coupon.StatusCode,
                            ErrorMessage = coupon.ErrorMessage,
                        };
                    }
                }


                return new ResultViewModel<GetCouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ResultViewModel<GetCouponViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ResultViewModel<GetCouponViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ResultViewModel<GetCouponViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }
        
        public async Task<ResultViewModel<int>> CreateCouponAsync(CreateCouponViewModel createCoupon)
        {
            AddBearerToken();
            try
            {
                CreateCouponDto createCouponDto = mapper.Map<CreateCouponDto>(createCoupon);
                var apiResponse = await _couponClient.CreateCouponAsync(createCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel<int>
                    {
                        Data = apiResponse.Data,
                        SuccessCode = apiResponse.StatusCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ResultViewModel<int>
                        {
                            ValidationErrors = [error],
                            ErrorMessage = apiResponse.ErrorMessage,
                            ErrorCode = apiResponse.StatusCode,
                        };
                    }
                }

                return new ResultViewModel<int>();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ResultViewModel<int>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ResultViewModel<int>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ResultViewModel<int>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }
        
        public async Task<ResultViewModel> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon)
        {
            AddBearerToken();
            try
            {
                UpdateCouponDto updateCouponDto = mapper.Map<UpdateCouponDto>(updateCoupon);
                var apiResponse = await _couponClient.UpdateCouponAsync(updateCouponDto.CouponId, updateCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel
                    {
                        SuccessCode = apiResponse.StatusCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ResultViewModel
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.StatusCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new ResultViewModel();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ResultViewModel
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ResultViewModel
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ResultViewModel
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<ResultViewModel> DeleteCouponAsync(int couponId, DeleteCouponViewModel deleteCoupon)
        {
            AddBearerToken();
            try
            {
                DeleteCouponDto deleteCouponDto = mapper.Map<DeleteCouponDto>(deleteCoupon);
                var apiResponse = await _couponClient.DeleteCouponAsync(deleteCouponDto.CouponId, deleteCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel
                    {
                        SuccessCode = apiResponse.StatusCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ResultViewModel
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.StatusCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new ResultViewModel
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new ResultViewModel
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new ResultViewModel
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<CollectionResultViewModel<bool>> DeleteCouponsAsync(DeleteCouponListViewModel deleteCouponList)
        {
            AddBearerToken();
            try
            {
                var deleteCouponListDto = mapper.Map<DeleteCouponsDto>(deleteCouponList);
                var apiResponse = await _couponClient.DeleteCouponListAsync(deleteCouponListDto);

                if (apiResponse.IsSuccess)
                {
                    return new CollectionResultViewModel<bool>
                    {
                        SuccessCode = apiResponse.StatusCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CollectionResultViewModel<bool>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.StatusCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new CollectionResultViewModel<bool>();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new CollectionResultViewModel<bool>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new CollectionResultViewModel<bool>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new CollectionResultViewModel<bool>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }
    }
}