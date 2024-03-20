using Identity.Application.Features.Users.Requests.Handlers;
using Identity.Application.Features.Users.Requests.Queries;
using Identity.Domain.DTOs.Authentications;
using Identity.Domain.DTOs.Identities;
using Identity.Domain.ResultIdentity;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IMediator mediator, ILogger<UserWithRolesDto> userWithRolesLogger, ILogger<DeleteUserDto> deleteLogger, ILogger<bool> boolLogger) : ControllerBase
    {
        private readonly IMediator _mediator = mediator;
        private readonly ILogger<UserWithRolesDto> _userWithRolesLogger = userWithRolesLogger;
        private readonly ILogger<DeleteUserDto> _deleteLogger = deleteLogger;
        private readonly ILogger<bool> _boolLogger = boolLogger;

        [HttpGet("GetAllUsers")]
        [Authorize(Roles = RoleConsts.Administrator)]
        public async Task<ActionResult<CollectionResult<UserWithRolesDto>>> GetAllUsers(string userId = "")
        {
            var command = await _mediator.Send(new GetUserListRequest() { UserId = userId });

            if (command.IsSuccess)
            {
                _userWithRolesLogger.LogDebug($"LogDebug ================ Уcпешный вывод пользователей: {userId}");
                return Ok(command);
            }

            _userWithRolesLogger.LogError($"LogDebugError ================ Выод пользователей не удался: {userId}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        // GET api/<UserController>/5
        [Authorize]
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
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        [HttpPost("CreateUser")]
        [Authorize(Roles = RoleConsts.Administrator)]
        public async Task<ActionResult<Result<UserWithRolesDto>>> CreateUser([FromBody] CreateUserDto createUserDto)
        {
            var command = await _mediator.Send(new CreateUserRequest { CreateUser = createUserDto });

            if (command.IsSuccess)
            {
                _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно создан: {createUserDto.UserName}");
                return Ok(command);
            }

            _userWithRolesLogger.LogError($"LogDebugError ================ Удалить пользователея не удалось: {createUserDto.UserName}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }

        // PUT api/<UserController>/5
        [HttpPut("{userId}")]
        [Authorize(Roles = RoleConsts.Administrator)]
        public async Task<ActionResult<Result<RegisterResponseDto>>> Put(string userId, [FromBody] UpdateUserDto updateUser)
        {
            if (userId == updateUser.Id)
            {
                var command = await _mediator.Send(new UpdateUserRequest { UpdateUser = updateUser });

                if (command.IsSuccess)
                {
                    _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно обновлен: {updateUser.Id}");
                    return Ok(command);
                }

                _userWithRolesLogger.LogError($"LogDebugError ================ Обновить пользователеля не удалось: {updateUser.Id}");
                foreach (var error in command.ValidationErrors!)
                {
                    return BadRequest(error);
                }

                return NoContent();
            }

            return NoContent();
        }

        // PUT api/<UserController>/5
        [Authorize]
        [HttpPut("UpdateUserProfile/{userId}")]
        public async Task<ActionResult<Result<UserWithRolesDto>>> UpdateUserProfile(string userId, [FromForm] UpdateUserProfileDto updateUserProfile)
        {
            if (userId == updateUserProfile.Id)
            {
                var command = await _mediator.Send(new UpdateUserProfileRequest { UpdateUserProfile = updateUserProfile });

                if (command.IsSuccess)
                {
                    _userWithRolesLogger.LogDebug($"LogDebug ================ Профиль пользователя успешно обновлен: {updateUserProfile.Id}");
                    return Ok(command);
                }

                _userWithRolesLogger.LogError($"LogDebugError ================ Обновить профиль пользователеля не удалось: {updateUserProfile.Id}");
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

        // DELETE api/<UserController>/5
        [HttpDelete("{userId}")]
        [Authorize(Roles = RoleConsts.Administrator)]
        public async Task<ActionResult<Result<DeleteUserDto>>> Delete(string userId, [FromBody] DeleteUserDto deleteUserDto)
        {
            if (userId == deleteUserDto.Id)
            {
                var command = await _mediator.Send(new DeleteUserRequest { DeleteUser = deleteUserDto });

                if (command.IsSuccess)
                {
                    _deleteLogger.LogDebug($"LogDebug ================ Пользователя успешно удален: {deleteUserDto.Id}");
                    return Ok(command);
                }

                _deleteLogger.LogError($"LogDebugError ================ Удалить пользователеля не удалось: {deleteUserDto.Id}");
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

        [HttpDelete()]
        [Authorize(Roles = RoleConsts.Administrator)]
        public async Task<ActionResult<Result<bool>>> Delete([FromBody] DeleteUserListDto deleteUserListDto)
        {
            var command = await _mediator.Send(new DeleteUserListRequest { DeleteUserList = deleteUserListDto });

            if (command.IsSuccess)
            {
                _boolLogger.LogDebug($"LogDebug ================ Пользователи успешно удалены: {deleteUserListDto.UserIds}");
                return Ok(command);
            }

            _deleteLogger.LogError($"LogDebugError ================ Удалить пользователелей не удалось: {deleteUserListDto.UserIds}");
            foreach (var error in command.ValidationErrors!)
            {
                return BadRequest(error);
            }

            return NoContent();
        }
    }
}