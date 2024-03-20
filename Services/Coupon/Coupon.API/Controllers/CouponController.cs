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
    [Authorize(Roles = StaticDetails.RoleAdministrator)]
    public class CouponController(IMediator mediator, ILogger<CouponDto> logger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<CouponDto> _logger = logger;

        // GET: api/<CouponController>
        [HttpGet("Coupons")]
        public async Task<ActionResult<CollectionResult<CouponDto>>> GetCoupons()
        {
            var query = await _mediator.Send(new GetCouponListRequest());

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
        [HttpGet("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> GetCoupon(int id)
        {
            var query = await _mediator.Send(new GetCouponDetailsRequest() { Id = id });

            if (query.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно получен: {id}");
                return Ok(query);
            }

            _logger.LogError($"LogDebugError ================ Ошибка получения купона: {id}");
            foreach (var error in query.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        [HttpGet("GetCouponByCode/{couponCode}")]
        public async Task<ActionResult<Result<CouponDto>>> GetCouponByCode(string couponCode)
        {
            var query = await _mediator.Send(new GetCouponDetailsByCouponNameRequest { CouponCode = couponCode });

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
        public async Task<ActionResult<Result<CouponDto>>> Post([FromBody] CreateCouponDto createCouponDto)
        {
            var command = await _mediator.Send(new CreateCouponRequest { CreateCoupon = createCouponDto });

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
        public async Task<ActionResult<Result<CouponDto>>> Put(int id, [FromBody] UpdateCouponDto updateCouponDto)
        {
            if (id == updateCouponDto.CouponId)
            {
                var command = await _mediator.Send(new UpdateCouponRequest { UpdateCoupon = updateCouponDto });

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
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> Delete(int id, [FromBody] DeleteCouponDto deleteCouponDto)
        {
            if (id == deleteCouponDto.Id)
            {
                var command = await _mediator.Send(new DeleteCouponRequest { DeleteCoupon = deleteCouponDto });

                if (command.IsSuccess)
                {
                    _logger.LogDebug($"LogDebug ================ Купон успешно удален: {deleteCouponDto.Id}");
                    return Ok(command);
                }

                _logger.LogError($"LogDebugError ================ Ошибка удаления купона: {deleteCouponDto.Id}");
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
        public async Task<ActionResult<CollectionResult<CouponDto>>> DeleteCouponList([FromBody] DeleteCouponListDto deleteCouponListDto)
        {
            var command = await _mediator.Send(new DeleteCouponListRequest { DeleteCoupon = deleteCouponListDto });

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