using AutoMapper;
using Azure.Core;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Resources.Features.Coupons.Handlers.Commands;
using Coupon.Application.Validations;
using Coupon.Domain.DTOs;
using Coupon.Domain.Entities;
using Coupon.Domain.Enum;
using Coupon.Domain.Interfaces.Repositories;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Serilog;
using System.Net.Http;

namespace Coupon.Application.Features.Coupons.Handlers.Commands
{
    public class CreateCouponRequestHandler(IBaseRepository<CouponEntity> repository, ILogger logger, IMapper mapper, IStringLocalizer<ErrorMessage> localizer, IHttpContextAccessor httpContextAccessor, CreateValidator createValidator) : IRequestHandler<CreateCouponRequest, Result<CouponDto>>
    {
        private readonly IBaseRepository<CouponEntity> _repository = repository;
        private readonly ILogger _logger = logger.ForContext<CreateCouponRequestHandler>();
        private readonly IMapper _mapper = mapper;
        private readonly IStringLocalizer<ErrorMessage> _localizer = localizer;
        private readonly CreateValidator _createValidator = createValidator;
        private readonly IHttpContextAccessor _httpContextAccessor = httpContextAccessor;

        public async Task<Result<CouponDto>> Handle(CreateCouponRequest request, CancellationToken cancellationToken)
        {
            try
            {
                var result = await _createValidator.ValidateAsync(request.CreateCoupon, cancellationToken);

                if (!result.IsValid)
                {
                    var errorMessages = new Dictionary<string, List<string>>
                    {
                        { "CouponCode", result.Errors.Select(key => key.ErrorMessage).ToList() },
                        { "DiscountAmount", result.Errors.Select(key => key.ErrorMessage).ToList() },
                        { "MinAmount", result.Errors.Select(key => key.ErrorMessage).ToList() }
                    };

                    foreach (var error in errorMessages)
                    {
                        if (errorMessages.TryGetValue(error.Key, out var errorMessage))
                        {
                            return new Result<CouponDto>
                            {
                                ValidationErrors = errorMessage,
                                ErrorMessage = ErrorMessage.CouponNotCreated,
                                ErrorCode = (int)ErrorCodes.CouponNotCreated,
                            };
                        }
                    }

                    return new Result<CouponDto>
                    {
                        ErrorMessage = ErrorMessage.CouponNotCreated,
                        ErrorCode = (int)ErrorCodes.CouponNotCreated,
                        ValidationErrors = result.Errors.Select(key => key.ErrorMessage).ToList(),
                    };
                }

                else
                {
                    var coupon = await _repository.GetAllAsync().FirstOrDefaultAsync(key => key.CouponCode == request.CreateCoupon.CouponCode, cancellationToken);

                    if (coupon is not null)
                    {
                        string errorMessageKey = "";

                        var encodedCookieValues = "c%3Den-US%7Cuic%3Den-US,c%3Dru-RU%7Cuic%3Dru-RU";

                        var decodedCookieValues = Uri.UnescapeDataString(encodedCookieValues);
                        var cookieValues = decodedCookieValues.Split(',');

                        var selectedLanguage = _httpContextAccessor.HttpContext.Features.Get<IRequestCultureFeature>().RequestCulture.Culture.Name;

                        foreach (var cookieValue in cookieValues)
                        {
                            var cultureParts = cookieValue.Trim().Split('=')[1];
                            var culture = cultureParts[..5];

                            if (culture == selectedLanguage)
                            {
                                errorMessageKey = ErrorMessage_en_US1.CouponAlreadyExists;
                                break;
                            }

                            if (culture == selectedLanguage)
                            {
                                errorMessageKey = ErrorMessage_ru_RU1.CouponAlreadyExists;
                                break;
                            }
                        }

                        return new Result<CouponDto>
                        {
                            Data = null,
                            ErrorCode = (int)ErrorCodes.CouponAlreadyExists,
                            ErrorMessage = _localizer[errorMessageKey].Value,
                        };
                    }

                    else
                    {
                        coupon = new CouponEntity
                        {
                            CouponCode = request.CreateCoupon.CouponCode.Replace(" ", ""),
                            DiscountAmount = request.CreateCoupon.DiscountAmount,
                            MinAmount = request.CreateCoupon.MinAmount,
                        };

                        await _repository.CreateAsync(coupon);

                        var options = new Stripe.CouponCreateOptions
                        {
                            Currency = "byn",
                            Id = coupon.CouponCode,
                            Name = coupon.CouponCode,
                            AmountOff = (long)(coupon.DiscountAmount * 100),
                        };

                        var service = new Stripe.CouponService();
                        service.Create(options);

                        return new Result<CouponDto>
                        {
                            Data = _mapper.Map<CouponDto>(coupon),
                            SuccessMessage = "Купон успешно создан",
                        };
                    }
                }
            }

            catch (Exception exception)
            {
                _logger.Error(exception, exception.Message);
                return new Result<CouponDto>
                {
                    ErrorMessage = ErrorMessage.InternalServerError,
                    ErrorCode = (int)ErrorCodes.InternalServerError,
                };
            }
        }
    }
}