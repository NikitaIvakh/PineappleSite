using Identity.Application.Features.Identities.Requests.Commands;
using Identity.Application.Features.Identities.Requests.Queries;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator, ILogger<UserWithRolesDto> userWithRolesLogger, ILogger<DeleteUserDto> deleteLogger, ILogger<bool> boolLogger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<UserWithRolesDto> _userWithRolesLogger = userWithRolesLogger;
        private readonly ILogger<DeleteUserDto> _deleteLogger = deleteLogger;
        private readonly ILogger<bool> _boolLogger = boolLogger;

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<CollectionResult<UserWithRolesDto>>> GetAllUsers(string userId = "")
        {
            var command = await _mediator.Send(new GetUserListRequest() { UserId = userId });

            if (command.IsSuccess)
            {
                _userWithRolesLogger.LogDebug($"LogDebug ================ Уcпешный вывод пользователей: {userId}");
                return Ok(command);
            }

            _userWithRolesLogger.LogError($"LogDebugError ================ Выод пользователей не удался: {userId}");
            return BadRequest(command.ErrorMessage);
        }

        // GET api/<UserController>/5
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<Result<UserWithRolesDto>>> GetUserById(string id)
        {
            var command = await _mediator.Send(new GetUserDetailsRequest { Id = id });

            if (command.IsSuccess)
            {
                _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно получен: {id}");
                return Ok(command);
            }

            _userWithRolesLogger.LogError($"LogDebugError ================ Получить пользователея не удалось: {id}");
            return BadRequest(command.ErrorMessage);
        }

        [HttpPost("CreateUser")]
        public async Task<ActionResult<Result<UserWithRolesDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = await _mediator.Send(new CreateUserRequest { CreateUser = createUserDto });

            if (command.IsSuccess)
            {
                _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно создан: {createUserDto.UserName}");
                return Ok(command);
            }

            _userWithRolesLogger.LogError($"LogDebugError ================ Удалить пользователея не удалось: {createUserDto.UserName}");
            return BadRequest(command.ValidationErrors);
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public async Task<ActionResult<Result<RegisterResponseDto>>> Put([FromBody] UpdateUserDto updateUser)
        {
            var command = await _mediator.Send(new UpdateUserRequest { UpdateUser = updateUser });

            if (command.IsSuccess)
            {
                _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно обновлен: {updateUser.Id}");
                return Ok(command);
            }

            _userWithRolesLogger.LogError($"LogDebugError ================ Обновить пользователеля не удалось: {updateUser.Id}");
            return BadRequest(command.ValidationErrors);
        }

        // PUT api/<UserController>/5
        [HttpPut("UpdateUserProfile/{userId}")]
        public async Task<ActionResult<Result<UserWithRolesDto>>> UpdateUserProfile([FromForm] UpdateUserProfileDto updateUserProfile)
        {
            var command = await _mediator.Send(new UpdateUserProfileRequest { UpdateUserProfile = updateUserProfile });

            if (command.IsSuccess)
            {
                _userWithRolesLogger.LogDebug($"LogDebug ================ Профиль пользователя успешно обновлен: {updateUserProfile.Id}");
                return Ok(command);
            }

            _userWithRolesLogger.LogError($"LogDebugError ================ Обновить профиль пользователеля не удалось: {updateUserProfile.Id}");
            return BadRequest(command.ValidationErrors);
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Result<DeleteUserDto>>> Delete([FromBody] DeleteUserDto deleteUserDto)
        {
            var command = await _mediator.Send(new DeleteUserRequest { DeleteUser = deleteUserDto });

            if (command.IsSuccess)
            {
                _deleteLogger.LogDebug($"LogDebug ================ Пользователя успешно удален: {deleteUserDto.Id}");
                return Ok(command);
            }

            _deleteLogger.LogError($"LogDebugError ================ Удалить пользователеля не удалось: {deleteUserDto.Id}");
            return BadRequest(command.ErrorMessage);
        }

        [HttpDelete()]
        public async Task<ActionResult<Result<bool>>> Delete([FromBody] DeleteUserListDto deleteUserListDto)
        {
            var comamnd = await _mediator.Send(new DeleteUserListRequest { DeleteUserList = deleteUserListDto });

            if (comamnd.IsSuccess)
            {
                _boolLogger.LogDebug($"LogDebug ================ Пользователи успешно удалены: {deleteUserListDto.UserIds}");
                return Ok(comamnd);
            }

            _deleteLogger.LogError($"LogDebugError ================ Удалить пользователелей не удалось: {deleteUserListDto.UserIds}");
            return BadRequest(comamnd.ErrorMessage);
        }
    }
}