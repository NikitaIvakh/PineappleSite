using Coupon.API.Utility;
using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coupon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CouponController(IMediator mediator, ILogger<CouponDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<CouponDto> _logger = logger;

        // GET: api/<CouponController>
        [HttpGet("Coupons")]
        // [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<CollectionResult<CouponDto>>> GetCoupons()
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
        [HttpGet("{couponId}")]
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
        public async Task<ActionResult<Result<CouponDto>>> GetCouponByCode(string couponCode)
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
        [HttpPost]
        // [Authorize(Roles = StaticDetails.RoleAdministrator)]
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
        [HttpPut("{id}")]
       // [Authorize(Roles = StaticDetails.RoleAdministrator)]
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
        [HttpDelete("{couponId}")]
        // [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<Result<Unit>>> Delete(int couponId, [FromBody] DeleteCouponDto deleteCouponDto)
        {
            if (couponId == deleteCouponDto.CouponId)
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
        // [Authorize(Roles = StaticDetails.RoleAdministrator)]
        public async Task<ActionResult<CollectionResult<Unit>>> DeleteCouponList([FromBody] DeleteCouponsDto deleteCouponsDto)
        {
            var command = await _mediator.Send(new DeleteCouponsRequest(deleteCouponsDto));

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купоны успешно удалены: {deleteCouponsDto.CouponIds}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления купонов: {deleteCouponsDto.CouponIds}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }
    }
}