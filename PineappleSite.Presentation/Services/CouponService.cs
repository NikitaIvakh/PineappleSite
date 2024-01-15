using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Coupons;
using PineappleSite.Presentation.Services.Coupons;
using System;

namespace PineappleSite.Presentation.Services
{
    public class CouponService(ILocalStorageService localStorageService, ICouponClient couponClient, IMapper mapper) : BaseCouponService(localStorageService, couponClient), ICouponService
    {
        private readonly ILocalStorageService _localStorageService = localStorageService;
        private readonly ICouponClient _couponClient = couponClient;
        private readonly IMapper _mapper = mapper;

        public async Task<IEnumerable<CouponViewModel>> GetAllCouponsAsync()
        {
            var coupons = await _couponClient.CouponAllAsync();
            return _mapper.Map<IEnumerable<CouponViewModel>>(coupons);
        }

        public async Task<CouponViewModel> GetCouponAsync(int couponId)
        {
            var coupon = await _couponClient.CouponGETAsync(couponId);
            return _mapper.Map<CouponViewModel>(coupon);
        }

        public async Task<ResponseViewModel> CreateCouponAsync(CreateCouponViewModel createCoupon)
        {
            try
            {
                ResponseViewModel responseView = new();
                CreateCouponDto createCouponDto = _mapper.Map<CreateCouponDto>(createCoupon);
                BaseCommandResponse apiResponse = await _couponClient.CouponPOSTAsync(createCouponDto);

                if (apiResponse.IsSuccess)
                {
                    responseView.IsSuccess = true;
                    responseView.Id = apiResponse.Id;
                    responseView.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        responseView.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return responseView;
            }

            catch (CouponExceptions exceptions)
            {
                return ConvertCouponExceptions(exceptions);
            }
        }

        public async Task<ResponseViewModel> UpdateCouponAsync(int id, UpdateCouponViewModel updateCoupon)
        {
            try
            {
                ResponseViewModel responseView = new();
                UpdateCouponDto updateCouponDto = _mapper.Map<UpdateCouponDto>(updateCoupon);
                BaseCommandResponse apiResponse = await _couponClient.CouponPUTAsync(updateCouponDto.CouponId.ToString(), updateCouponDto);

                if (apiResponse.IsSuccess)
                {
                    responseView.IsSuccess = true;
                    responseView.Id = apiResponse.Id;
                    responseView.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        responseView.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return responseView;
            }

            catch (CouponExceptions exceptions)
            {
                return ConvertCouponExceptions(exceptions);
            }
        }

        public async Task<ResponseViewModel> DeleteCouponAsync(int id, DeleteCouponViewModel deleteCoupon)
        {
            try
            {
                ResponseViewModel responseView = new();
                DeleteCouponDto deleteCouponDto = _mapper.Map<DeleteCouponDto>(deleteCoupon);
                BaseCommandResponse apiResponse = await _couponClient.CouponDELETE2Async(deleteCouponDto.Id.ToString(), deleteCouponDto);

                if (apiResponse.IsSuccess)
                {
                    responseView.IsSuccess = true;
                    responseView.Id = apiResponse.Id;
                    responseView.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        responseView.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return responseView;
            }

            catch (CouponExceptions exceptions)
            {
                return ConvertCouponExceptions(exceptions);
            }
        }

        public async Task<ResponseViewModel> DeleteCouponsAsync(DeleteCouponListViewModel deleteCouponList)
        {
            try
            {
                ResponseViewModel response = new();
                DeleteCouponListDto deleteCouponListDto = _mapper.Map<DeleteCouponListDto>(deleteCouponList);
                BaseCommandResponse apiResponse = await _couponClient.CouponDELETEAsync(deleteCouponListDto);

                if (apiResponse.IsSuccess)
                {
                    response.IsSuccess = true;
                    response.Id = apiResponse.Id;
                    response.Message = apiResponse.Message;
                }

                else
                {
                    foreach (string error in apiResponse.ValidationErrors)
                    {
                        response.ValidationErrors += error + Environment.NewLine;
                    }
                }

                return response;
            }

            catch (CouponExceptions exceptions)
            {
                return ConvertCouponExceptions(exceptions);
            }
        }
    }
}