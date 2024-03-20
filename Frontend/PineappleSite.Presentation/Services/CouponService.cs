using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Services
{
    public class CouponService(ILocalStorageService localStorageService, ICouponClient couponClient, IMapper mapper, IHttpContextAccessor httpContextAccessor) : BaseCouponService(localStorageService, couponClient, httpContextAccessor), ICouponService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;
        private readonly ICouponClient _couponClient = couponClient;
        private readonly IMapper _mapper = mapper;

        public async Task<CollectionResultViewModel<CouponViewModel>> GetAllCouponsAsync()
        {
            AddBearerToken();
            try
            {
                CouponDtoCollectionResult coupons = await _couponClient.CouponsAsync();

                if (coupons.IsSuccess)
                {
                    return _mapper.Map<CollectionResultViewModel<CouponViewModel>>(coupons);
                }

                else
                {
                    foreach (var error in coupons.ValidationErrors)
                    {
                        return new CollectionResultViewModel<CouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = coupons.ErrorCode,
                            ErrorMessage = coupons.ErrorMessage,
                        };
                    }
                }

                return new CollectionResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                if (exceptions.StatusCode == 403)
                {
                    return new CollectionResultViewModel<CouponViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else if (exceptions.StatusCode == 401)
                {
                    return new CollectionResultViewModel<CouponViewModel>
                    {
                        ErrorCode = exceptions.StatusCode,
                        ErrorMessage = exceptions.Response,
                        ValidationErrors = ConvertCouponExceptions(exceptions).ValidationErrors,
                    };
                }

                else
                {
                    return new CollectionResultViewModel<CouponViewModel>
                    {
                        ErrorMessage = exceptions.Response,
                        ErrorCode = exceptions.StatusCode,
                        ValidationErrors = [exceptions.Response]
                    };
                }
            }
        }

        public async Task<ResultViewModel<CouponViewModel>> GetCouponAsync(int couponId)
        {
            AddBearerToken();
            try
            {
                CouponDtoResult coupon = await _couponClient.CouponGETAsync(couponId);

                if (coupon.IsSuccess)
                {
                    return new ResultViewModel<CouponViewModel>
                    {
                        SuccessCode = coupon.SuccessCode,
                        SuccessMessage = coupon.SuccessMessage,
                        Data = _mapper.Map<CouponViewModel>(coupon.Data),
                    };
                }

                else
                {
                    foreach (var error in coupon.ValidationErrors)
                    {
                        return new ResultViewModel<CouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = coupon.ErrorCode,
                            ErrorMessage = coupon.ErrorMessage,
                        };
                    }
                }

                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<ResultViewModel<CouponViewModel>> GetCouponAsync(string couponCode)
        {
            AddBearerToken();
            try
            {
                CouponDtoResult coupon = await _couponClient.GetCouponByCodeAsync(couponCode);

                if (coupon.IsSuccess)
                {
                    return _mapper.Map<ResultViewModel<CouponViewModel>>(coupon.Data);
                }

                else
                {
                    foreach (var error in coupon.ValidationErrors)
                    {
                        return new ResultViewModel<CouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = coupon.ErrorCode,
                            ErrorMessage = coupon.ErrorMessage,
                        };
                    }
                }


                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<ResultViewModel<CouponViewModel>> CreateCouponAsync(CreateCouponViewModel createCoupon)
        {
            AddBearerToken();
            try
            {
                CreateCouponDto createCouponDto = _mapper.Map<CreateCouponDto>(createCoupon);
                CouponDtoResult apiResponse = await _couponClient.CouponPOSTAsync(createCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel<CouponViewModel>
                    {
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CouponViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ResultViewModel<CouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorMessage = apiResponse.ErrorMessage,
                            ErrorCode = apiResponse.ErrorCode,
                        };
                    }
                }

                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<ResultViewModel<CouponViewModel>> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon)
        {
            AddBearerToken();
            try
            {
                UpdateCouponDto updateCouponDto = _mapper.Map<UpdateCouponDto>(updateCoupon);
                CouponDtoResult apiResponse = await _couponClient.CouponPUTAsync(updateCouponDto.CouponId, updateCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel<CouponViewModel>
                    {
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CouponViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ResultViewModel<CouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<ResultViewModel<CouponViewModel>> DeleteCouponAsync(int id, DeleteCouponViewModel deleteCoupon)
        {
            AddBearerToken();
            try
            {
                DeleteCouponDto deleteCouponDto = _mapper.Map<DeleteCouponDto>(deleteCoupon);
                CouponDtoResult apiResponse = await _couponClient.CouponDELETEAsync(deleteCouponDto.Id, deleteCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel<CouponViewModel>
                    {
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<CouponViewModel>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new ResultViewModel<CouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }

        public async Task<CollectionResultViewModel<CouponViewModel>> DeleteCouponsAsync(DeleteCouponListViewModel deleteCouponList)
        {
            AddBearerToken();
            try
            {
                DeleteCouponListDto deleteCouponListDto = _mapper.Map<DeleteCouponListDto>(deleteCouponList);
                CouponDtoCollectionResult apiResponse = await _couponClient.DeleteCouponListAsync(deleteCouponListDto);

                if (apiResponse.IsSuccess)
                {
                    return new CollectionResultViewModel<CouponViewModel>
                    {
                        SuccessCode = apiResponse.SuccessCode,
                        SuccessMessage = apiResponse.SuccessMessage,
                        Data = _mapper.Map<IReadOnlyCollection<CouponViewModel>>(apiResponse.Data),
                    };
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        return new CollectionResultViewModel<CouponViewModel>
                        {
                            ValidationErrors = [error],
                            ErrorCode = apiResponse.ErrorCode,
                            ErrorMessage = apiResponse.ErrorMessage,
                        };
                    }
                }

                return new CollectionResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new CollectionResultViewModel<CouponViewModel>
                {
                    ErrorCode = exceptions.StatusCode,
                    ErrorMessage = exceptions.Response,
                    ValidationErrors = [exceptions.Response]
                };
            }
        }
    }
}