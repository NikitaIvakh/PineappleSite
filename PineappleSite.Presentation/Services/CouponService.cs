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
            CouponDtoCollectionResult coupons = await _couponClient.CouponsAsync();
            return _mapper.Map<CollectionResultViewModel<CouponViewModel>>(coupons);
        }

        public async Task<CouponViewModel> GetCouponAsync(int couponId)
        {
            AddBearerToken();
            CouponDtoResult coupon = await _couponClient.CouponGETAsync(couponId);
            return _mapper.Map<CouponViewModel>(coupon.Data);
        }

        public async Task<ResultViewModel<CouponViewModel>> GetCouponAsync(string couponCode)
        {
            AddBearerToken();
            CouponDtoResult coupon = await _couponClient.GetCouponByCodeAsync(couponCode);
            return _mapper.Map<ResultViewModel<CouponViewModel>>(coupon.Data);
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