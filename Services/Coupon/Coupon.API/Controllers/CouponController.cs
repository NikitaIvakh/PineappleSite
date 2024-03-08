using Coupon.Application.Features.Coupons.Requests.Commands;
using Coupon.Application.Features.Coupons.Requests.Queries;
using Coupon.Domain.DTOs;
using Coupon.Domain.ResultCoupon;
using MediatR;
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
        public async Task<ActionResult<CollectionResult<CouponDto>>> GetCoupons()
        {
            var query = await _mediator.Send(new GetCouponListRequest());
            if (query.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купоны успешно получены");
                return Ok(query);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения купонов");
            return BadRequest(query.ErrorMessage);
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
            return BadRequest(query.ErrorMessage);
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
            return BadRequest(query.ErrorMessage);
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
            return BadRequest(command.ErrorMessage);
        }

        // PUT api/<CouponController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> Put([FromBody] UpdateCouponDto updateCouponDto)
        {
            var command = await _mediator.Send(new UpdateCouponRequest { UpdateCoupon = updateCouponDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно обновлен: {updateCouponDto.CouponId}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка обновления купона: {updateCouponDto.CouponId}");
            return BadRequest(command.ValidationErrors);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> Delete([FromBody] DeleteCouponDto deleteCouponDto)
        {
            var command = await _mediator.Send(new DeleteCouponRequest { DeleteCoupon = deleteCouponDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug($"LogDebug ================ Купон успешно удален: {deleteCouponDto.Id}");
                return Ok(command);
            }

            _logger.LogError($"LogDebugError ================ Ошибка удаления купона: {deleteCouponDto.Id}");
            return BadRequest(command.ErrorMessage);
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
            return BadRequest(command.ErrorMessage);
        }
    }
}