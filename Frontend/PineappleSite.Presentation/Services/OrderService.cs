using AutoMapper;
using PineappleSite.Presentation.Contracts;
using PineappleSite.Presentation.Models.Orders;
using PineappleSite.Presentation.Models.ShoppingCart;
using PineappleSite.Presentation.Services.Orders;

namespace PineappleSite.Presentation.Services;

public sealed class OrderService(
    ILocalStorageService localStorageService,
    IOrderClient orderClient,
    IMapper mapper,
    IHttpContextAccessor contextAccessor)
    : BaseOrderService(localStorageService, orderClient, contextAccessor), IOrderService
{
    private readonly IOrderClient _orderClient = orderClient;

    public async Task<OrderCollectionResult<OrderHeaderViewModel>> GetAllOrdersAsync(string userId)
    {
        AddBearerToken();
        try
        {
            var apiResponse = await _orderClient.GetOrdersAsync(userId);

            if (apiResponse.IsSuccess)
            {
                return new OrderCollectionResult<OrderHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<IReadOnlyCollection<OrderHeaderViewModel>>(apiResponse.Data),
                };
            }

            return new OrderCollectionResult<OrderHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (OrdersExceptions<string> exceptions)
        {
            return new OrderCollectionResult<OrderHeaderViewModel>
            {
                StatusCode = 403,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<OrderResult<OrderHeaderViewModel>> GetOrderAsync(int orderHeaderId)
    {
        AddBearerToken();
        try
        {
            var apiResponse = await _orderClient.GetOrderAsync(orderHeaderId);

            if (apiResponse.IsSuccess)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<OrderHeaderViewModel>(apiResponse.Data),
                };
            }

            return new OrderResult<OrderHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (OrdersExceptions<string> exceptions)
        {
            return new OrderResult<OrderHeaderViewModel>()
            {
                ErrorMessage = exceptions.Result,
                StatusCode = exceptions.StatusCode,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<OrderResult<OrderHeaderViewModel>> CreateOrderAsync(CartViewModel cartViewModel)
    {
        AddBearerToken();
        try
        {
            var cartDto = mapper.Map<CartDto>(cartViewModel);
            var apiResponse = await _orderClient.CreateOrderAsync(cartDto);

            if (apiResponse.IsSuccess)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<OrderHeaderViewModel>(apiResponse.Data),
                };
            }

            return new OrderResult<OrderHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (OrdersExceptions<string> exceptions)
        {
            return new OrderResult<OrderHeaderViewModel>()
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<OrderResult<StripeRequestViewModel>> CreateStripeSessionAsync(
        StripeRequestViewModel stripeRequest)
    {
        AddBearerToken();
        try
        {
            var stripeRequestDto = mapper.Map<StripeRequestDto>(stripeRequest);
            var apiRespone = await _orderClient.CreateStripeSessionAsync(stripeRequestDto);

            if (apiRespone.IsSuccess)
            {
                return new OrderResult<StripeRequestViewModel>
                {
                    StatusCode = apiRespone.StatusCode,
                    SuccessMessage = apiRespone.SuccessMessage,
                    Data = mapper.Map<StripeRequestViewModel>(apiRespone.Data),
                };
            }

            return new OrderResult<StripeRequestViewModel>
            {
                StatusCode = apiRespone.StatusCode,
                ErrorMessage = apiRespone.ErrorMessage,
                ValidationErrors = string.Join(", ", apiRespone.ValidationErrors),
            };
        }

        catch (OrdersExceptions<string> exceptions)
        {
            return new OrderResult<StripeRequestViewModel>()
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result
            };
        }
    }

    public async Task<OrderResult<OrderHeaderViewModel>> ValidateStripeSessionAsync(
        ValidateStripSessionViewModel validateStripSessionViewModel)
    {
        AddBearerToken();
        try
        {
            var validateStripeSessionDto = mapper.Map<ValidateStripeSessionDto>(validateStripSessionViewModel);
            var apiResponse = await _orderClient.ValidateStripeSessionAsync(validateStripeSessionDto);

            if (apiResponse.IsSuccess)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<OrderHeaderViewModel>(apiResponse.Data),
                };
            }

            return new OrderResult<OrderHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (OrdersExceptions<string> exceptions)
        {
            return new OrderResult<OrderHeaderViewModel>()
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }

    public async Task<OrderResult<OrderHeaderViewModel>> UpdateOrderStatusAsync(
        UpdateOrderStatusViewModel updateOrderStatusViewModel)
    {
        AddBearerToken();
        try
        {
            var updateOrderStatusDto = mapper.Map<UpdateOrderStatusDto>(updateOrderStatusViewModel);
            var apiResponse = await _orderClient.UpdateOrderStatusAsync(updateOrderStatusDto);

            if (apiResponse.IsSuccess)
            {
                return new OrderResult<OrderHeaderViewModel>
                {
                    StatusCode = apiResponse.StatusCode,
                    SuccessMessage = apiResponse.SuccessMessage,
                    Data = mapper.Map<OrderHeaderViewModel>(apiResponse),
                };
            }

            return new OrderResult<OrderHeaderViewModel>
            {
                StatusCode = apiResponse.StatusCode,
                ErrorMessage = apiResponse.ErrorMessage,
                ValidationErrors = string.Join(", ", apiResponse.ValidationErrors),
            };
        }

        catch (OrdersExceptions<string> exceptions)
        {
            return new OrderResult<OrderHeaderViewModel>()
            {
                StatusCode = exceptions.StatusCode,
                ErrorMessage = exceptions.Result,
                ValidationErrors = exceptions.Result,
            };
        }
    }
}