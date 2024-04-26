// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
// using ShoppingCart.Application.Features.Requests.Commands;
// using ShoppingCart.Application.Features.Requests.Queries;
// using ShoppingCart.Domain.DTOs;
// using ShoppingCart.Domain.Results;
//
// namespace ShoppingCart.API.Controllers;
//
// [Authorize]
// [ApiController]
// [Route("api/[controller]")]
// public class ShoppingCartController(IMediator mediator, ILogger<CartDto> logger) : ControllerBase
// {
//     private readonly IMediator _mediator = mediator;
//     private readonly ILogger<CartDto> _logger = logger;
//
//     // GET: api/<ShoppingCartController>
//     [HttpGet("GetShoppingCart/{userId}")]
//     public async Task<ActionResult<Result<CartDto>>> GetShoppingCart(string userId)
//     {
//         var request = await _mediator.Send(new GetShoppingCartRequest { UserId = userId });
//
//         if (request.IsSuccess)
//         {
//             _logger.LogDebug($"LogDebug ================ Корзина успешно получена: {userId}");
//             return Ok(request);
//         }
//
//         _logger.LogError($"LogDebugError ================ Ошибка получения корзины: {userId}");
//         foreach (var error in request.ValidationErrors!)
//         {
//             return BadRequest(error);
//         }
//
//         return NoContent();
//     }
//
//     // POST api/<ShoppingCartController>
//     [HttpPost]
//     public async Task<ActionResult<Result<CartDto>>> Post([FromBody] CartDto cartDto)
//     {
//         var command = await _mediator.Send(new ShoppingCartUpsertRequest { CartDto = cartDto });
//
//         if (command.IsSuccess)
//         {
//             _logger.LogDebug(
//                 $"LogDebug ================ В корзину успешно добавлен товар: {cartDto.CartHeader.CartHeaderId}");
//             return Ok(command);
//         }
//
//         _logger.LogError(
//             $"LogDebugError ================ Ошибка добавления товара в корзину: {cartDto.CartHeader.CartHeaderId}");
//         foreach (var error in command.ValidationErrors!)
//         {
//             return BadRequest(error);
//         }
//
//         return NoContent();
//     }
//
//     [HttpPost("ApplyCoupon")]
//     public async Task<ActionResult<Result<CartDto>>> ApplyCoupon([FromBody] CartDto cartDto)
//     {
//         var command = await _mediator.Send(new ApplyCouponRequest { CartDto = cartDto });
//
//         if (command.IsSuccess)
//         {
//             _logger.LogDebug(
//                 $"LogDebug ================ Купон успешно применен: {cartDto.CartHeader.CartHeaderId}");
//             return Ok(command);
//         }
//
//         _logger.LogError(
//             $"LogDebugError ================ Ошибка применения купона: {cartDto.CartHeader.CartHeaderId}");
//         foreach (var error in command.ValidationErrors!)
//         {
//             return BadRequest(error);
//         }
//
//         return NoContent();
//     }
//
//     [HttpPost("RemoveCoupon")]
//     public async Task<ActionResult<Result<CartDto>>> RemoveCoupon([FromBody] CartDto cartDto)
//     {
//         var command = await _mediator.Send(new RemoveCouponRequest { CartDto = cartDto });
//
//         if (command.IsSuccess)
//         {
//             _logger.LogDebug($"LogDebug ================ Купон успешно удален: {cartDto.CartHeader.CartHeaderId}");
//             return Ok(command);
//         }
//
//         _logger.LogError(
//             $"LogDebugError ================ Ошибка удаления купона: {cartDto.CartHeader.CartHeaderId}");
//         foreach (var error in command.ValidationErrors!)
//         {
//             return BadRequest(error);
//         }
//
//         return NoContent();
//     }
//
//     // DELETE api/<ShoppingCartController>/5
//     [HttpDelete("{productId}")]
//     public async Task<ActionResult<Result<CartDto>>> RemoveDetails([FromBody] int productId)
//     {
//         var command = await _mediator.Send(new RemoveShoppingCartProductRequest { ProductId = productId });
//
//         if (command.IsSuccess)
//         {
//             _logger.LogDebug($"LogDebug ================ Продукт из корзины успешно удален: {productId}");
//             return Ok(command);
//         }
//
//         _logger.LogError($"LogDebugError ================ Ошибка удаления продукта из корзины: {productId}");
//         foreach (var error in command.ValidationErrors!)
//         {
//             return BadRequest(error);
//         }
//
//         return NoContent();
//     }
//
//     [HttpDelete("RemoveDetailsList")]
//     public async Task<ActionResult<Result<CartDto>>> RemoveDetailsList([FromBody] DeleteProductsDto deleteProductsDto)
//     {
//         var command = await _mediator.Send(new RemoveShoppingCartProductsRequest
//             { DeleteProductDto = deleteProductsDto });
//
//         if (command.IsSuccess)
//         {
//             _logger.LogDebug(
//                 $"LogDebug ================ Продукты из корзины успешно удалены: {deleteProductsDto.ProductIds}");
//             return Ok(command);
//         }
//
//         _logger.LogError(
//             $"LogDebugError ================ Ошибка удаления продуктов из корзины: {deleteProductsDto.ProductIds}");
//         foreach (var error in command.ValidationErrors!)
//         {
//             return BadRequest(error);
//         }
//
//         return NoContent();
//     }
//
//     [HttpPost("RabbitMQShoppingCartRequest")]
//     public async Task<ActionResult<Result<bool>>> RabbitMQShoppingCartRequest([FromBody] CartDto cartDto)
//     {
//         var command = await _mediator.Send(new RabbitMqSendRequest { CartDto = cartDto });
//
//         if (command.IsSuccess)
//         {
//             _logger.LogDebug(
//                 $"LogDebug ================ Письмо в очередь успешно доставлено: {cartDto.CartHeader.CartHeaderId}");
//             return Ok(command);
//         }
//
//         _logger.LogError(
//             $"LogDebugError ================ Ошибка добавления письма в очередь: {cartDto.CartHeader.CartHeaderId}");
//         foreach (var error in command.ValidationErrors!)
//         {
//             return BadRequest(error);
//         }
//
//         return NoContent();
//     }
// }