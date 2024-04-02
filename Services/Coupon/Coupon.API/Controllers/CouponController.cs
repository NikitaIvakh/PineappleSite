using Coupon.Application.Features.Coupons.Handlers.Queries;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Coupon.API.Utility;
using Coupon.Application.Features.Coupons.Requests.Commands;

namespace Coupon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController(IMediator mediator, ILogger<GetCouponDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<GetCouponDto> _logger = logger;

        // GET: api/<CouponController>
        [HttpGet("GetCoupons")]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<CollectionResult<GetCouponsDto>>> GetCoupons()
        {
            var query = await _mediator.Send(new GetCouponsRequest());

            if (query.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купоны успешно получены");
                return Ok(query);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения купонов");
            foreach (var error in query.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        // GET api/<CouponController>/5
        [HttpGet("GetCouponById/{couponId}")]
        public async Task<ActionResult<Result<GetCouponDto>>> GetCoupon(int couponId)
        {
            var query = await _mediator.Send(new GetCouponRequest(couponId));

            if (query.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно получен: {couponId}");
                return Ok(query);
            }

            _logger.LogError($"LogDebugError ================ Ошибка получения купона: {couponId}");
            foreach (var error in query.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        [HttpGet("GetCouponByCode/{couponCode}")]
        public async Task<ActionResult<Result<GetCouponDto>>> GetCouponByCode(string couponCode)
        {
            var query = await _mediator.Send(new GetCouponByCodeRequest(couponCode));

            if (query.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно получен: {couponCode}");
                return Ok(query);
            }

            _logger.LogError($"LogDebugError ================ Ошибка получения купона: {couponCode}");
            foreach (var error in query.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        // POST api/<CouponController>
        [HttpPost("CreateCoupon")]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<Result<int>>> Post([FromBody] CreateCouponDto createCouponDto)
        {
            var command = await _mediator.Send(new CreateCouponRequest(createCouponDto));

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно создан: {createCouponDto.CouponCode}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка создания купона: {createCouponDto.CouponCode}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        // PUT api/<CouponController>/5
        [HttpPut("UpdateCoupon/{id}")]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<Result<Unit>>> Put(int id, [FromBody] UpdateCouponDto updateCouponDto)
        {
            if (id == updateCouponDto.CouponId)
            {
                var command = await _mediator.Send(new UpdateCouponRequest(updateCouponDto));

                if (command.IsSuccess)
                {
                    _logger.LogDebug($"LogDebug ================ Купон успешно обновлен: {updateCouponDto.CouponId}");
                    return Ok(command);
                }

                _logger.LogError($"LogDebugError ================ Ошибка обновления купона: {updateCouponDto.CouponId}");

                foreach (var error in command.ValidationErrors!)
                {
                    return BadRequest(error);
                }

                return NoContent();
            }

            else
            {
                return NoContent();
            }
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("DeleteCoupon/{id}")]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<Result<Unit>>> Delete(int id, [FromBody] DeleteCouponDto deleteCouponDto)
        {
            if (id == deleteCouponDto.CouponId)
            {
                var command = await _mediator.Send(new DeleteCouponRequest(deleteCouponDto));

                if (command.IsSuccess)
                {
                    _logger.LogDebug($"LogDebug ================ Купон успешно удален: {deleteCouponDto.CouponId}");
                    return Ok(command);
                }

                _logger.LogError($"LogDebugError ================ Ошибка удаления купона: {deleteCouponDto.CouponId}");
                foreach (var error in command.ValidationErrors!)
                {
                    return BadRequest(error);
                }

                return NoContent();
            }

            else
            {
                return NoContent();
            }
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("DeleteCouponList")]
        [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<CollectionResult<bool>>> DeleteCouponList([FromBody] DeleteCouponsDto deleteCouponListDto)
        {
            var command = await _mediator.Send(new DeleteCouponsRequest(deleteCouponListDto));

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купоны успешно удалены: {deleteCouponListDto.CouponIds}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления купонов: {deleteCouponListDto.CouponIds}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }
    }
}