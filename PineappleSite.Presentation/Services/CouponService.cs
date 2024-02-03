using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;

namespace PineappleSite.Presentation.Services
{
    public class CouponService(ILocalStorageService localStorageService, ICouponClient couponClient, IMapper mapper) : BaseCouponService(localStorageService, couponClient), ICouponService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
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
                            ErrorCode = coupons.ErrorCode,
                            ErrorMessage = coupons.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
                        };
                    }
                }

                return new CollectionResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new CollectionResultViewModel<CouponViewModel>
                {
                    ErrorMessage = exceptions.Response,
                    ErrorCode = exceptions.StatusCode,
                };
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
                        Data = _mapper.Map<CouponViewModel>(coupon.Data),
                    };
                }

                else
                {
                    foreach (var error in coupon.ValidationErrors)
                    {
                        return new ResultViewModel<CouponViewModel>
                        {
                            ErrorCode = coupon.ErrorCode,
                            ErrorMessage = coupon.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
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
                            ErrorCode = coupon.ErrorCode,
                            ErrorMessage = coupon.ErrorMessage,
                            ValidationErrors = error + Environment.NewLine,
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
                            ErrorMessage = error,
                            ValidationErrors = error + Environment.NewLine,
                            ErrorCode = 407,
                        };
                    }
                }

                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    ErrorCode = 500,
                    ErrorMessage = exceptions.Response,
                };
            }
        }

        public async Task<ResultViewModel<CouponViewModel>> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon)
        {
            AddBearerToken();
            try
            {
                UpdateCouponDto updateCouponDto = _mapper.Map<UpdateCouponDto>(updateCoupon);
                CouponDtoResult apiResponse = await _couponClient.CouponPUTAsync(updateCouponDto.CouponId.ToString(), updateCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel<CouponViewModel>
                    {
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
                            ErrorMessage = error,
                            ErrorCode = 407,
                        };
                    }
                }

                return new ResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new ResultViewModel<CouponViewModel>
                {
                    ErrorCode = 500,
                    ErrorMessage = exceptions.Response,
                };
            }
        }

        public async Task<ResultViewModel<CouponViewModel>> DeleteCouponAsync(int id, DeleteCouponViewModel deleteCoupon)
        {
            AddBearerToken();
            try
            {
                DeleteCouponDto deleteCouponDto = _mapper.Map<DeleteCouponDto>(deleteCoupon);
                CouponDtoResult apiResponse = await _couponClient.CouponDELETEAsync(deleteCouponDto.Id.ToString(), deleteCouponDto);

                if (apiResponse.IsSuccess)
                {
                    return new ResultViewModel<CouponViewModel>
                    {
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
                            ErrorMessage = error,
                            ErrorCode = 407,
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
                    ErrorCode = 500,
                };
            }
        }

        public async Task<CollectionResultViewModel<CouponViewModel>> DeleteCouponsAsync(DeleteCouponListViewModel deleteCouponList)
        {
            AddBearerToken();
            try
            {
                DeleteCouponListDto deleteCouponListDto = _mapper.Map<DeleteCouponListDto>(deleteCouponList);
                CouponDtoCollectionResult apiResponse = await _couponClient.CouponDELETE2Async(deleteCouponListDto);

                if (apiResponse.IsSuccess)
                {
                    return new CollectionResultViewModel<CouponViewModel>
                    {
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
                            ErrorMessage = error,
                            ErrorCode = 407,
                        };
                    }
                }

                return new CollectionResultViewModel<CouponViewModel>();
            }

            catch (CouponExceptions exceptions)
            {
                return new CollectionResultViewModel<CouponViewModel>
                {
                    ErrorCode = 500,
                    ErrorMessage = exceptions.Response,
                };
            }
        }
    }
}