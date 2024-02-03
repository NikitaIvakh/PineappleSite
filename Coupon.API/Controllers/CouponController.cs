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
                _logger.LogDebug("LogDebug ================ Купон успешно получен");
                return Ok(query);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения купона");
            return BadRequest(query.ErrorMessage);
        }

        [HttpGet("GetCouponByCode/{couponCode}")]
        public async Task<ActionResult<Result<CouponDto>>> GetCouponByCode(string couponCode)
        {
            var query = await _mediator.Send(new GetCouponDetailsByCouponNameRequest { CouponCode = couponCode });

            if (query.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купон успешно получен");
                return Ok(query);
            }

            _logger.LogError("LogDebugError ================ Ошибка получения купона");
            return BadRequest(query.ErrorMessage);
        }

        // POST api/<CouponController>
        [HttpPost]
        public async Task<ActionResult<Result<CouponDto>>> Post([FromBody] CreateCouponDto createCouponDto)
        {
            var command = await _mediator.Send(new CreateCouponRequest { CreateCoupon = createCouponDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купон успешно создан");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка создания купона");
            return BadRequest(command.ErrorMessage);
        }

        // PUT api/<CouponController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> Put([FromBody] UpdateCouponDto updateCouponDto)
        {
            var command = await _mediator.Send(new UpdateCouponRequest { UpdateCoupon = updateCouponDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купон успешно обновлен");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка обновления купона");
            return BadRequest(command.ValidationErrors);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<CouponDto>>> Delete([FromBody] DeleteCouponDto deleteCouponDto)
        {
            var command = await _mediator.Send(new DeleteCouponRequest { DeleteCoupon = deleteCouponDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купон успешно удылен");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления купона");
            return BadRequest(command.ErrorMessage);
        }

        // DELETE api/<CouponController>/5
        [HttpDelete]
        public async Task<ActionResult<CollectionResult<CouponDto>>> Delete([FromBody] DeleteCouponListDto deleteCouponListDto)
        {
            var command = await _mediator.Send(new DeleteCouponListRequest { DeleteCoupon = deleteCouponListDto });

            if (command.IsSuccess)
            {
                _logger.LogDebug("LogDebug ================ Купоны успешно удалены");
                return Ok(command);
            }

            _logger.LogError("LogDebugError ================ Ошибка удаления купонов");
            return BadRequest(command.ErrorMessage);
        }
    }
}