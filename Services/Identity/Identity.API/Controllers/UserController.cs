﻿// using Identity.Application.Features.Users.Requests.Handlers;
// using Identity.Application.Features.Users.Requests.Queries;
// using Identity.Domain.DTOs.Authentications;
// using Identity.Domain.DTOs.Identities;
// using Identity.Domain.ResultIdentity;
// using MediatR;
// using Microsoft.AspNetCore.Authorization;
// using Microsoft.AspNetCore.Mvc;
//
// namespace Identity.API.Controllers
// {
//     [ApiController]
//     [Route("api/[controller]")]
//     public class UserController(IMediator mediator, ILogger<GetUserDto> userWithRolesLogger, ILogger<DeleteUserDto> deleteLogger, ILogger<bool> boolLogger) : ControllerBase
//     {
//         private readonly IMediator _mediator = mediator;
//         private readonly ILogger<GetUserDto> _userWithRolesLogger = userWithRolesLogger;
//         private readonly ILogger<DeleteUserDto> _deleteLogger = deleteLogger;
//         private readonly ILogger<bool> _boolLogger = boolLogger;
//
//         [HttpGet("GetAllUsers")]
//         [Authorize(Roles = RoleConst.Administrator)]
//         public async Task<ActionResult<CollectionResult<GetUsersDto>>> GetAllUsers()
//         {
//             var command = await _mediator.Send(new GetUsersRequest());
//
//             if (command.IsSuccess)
//             {
//                 _userWithRolesLogger.LogDebug($"LogDebug ================ Уcпешный вывод пользователей");
//                 return Ok(command);
//             }
//
//             _userWithRolesLogger.LogError($"LogDebugError ================ Выод пользователей не удался");
//             foreach (var error in command.ValidationErrors!)
//             {
//                 return BadRequest(error);
//             }
//
//             return NoContent();
//         }
//
//         // GET api/<UserController>/5
//         [HttpGet("GetUserById/{userId}")]
//         public async Task<ActionResult<Result<GetUserDto>>> GetUserById(string userId)
//         {
//             var command = await _mediator.Send(new GetUserRequest { UserId = userId });
//
//             if (command.IsSuccess)
//             {
//                 _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно получен: {userId}");
//                 return Ok(command);
//             }
//
//             _userWithRolesLogger.LogError($"LogDebugError ================ Получить пользователея не удалось: {userId}");
//             foreach (var error in command.ValidationErrors!)
//             {
//                 return BadRequest(error);
//             }
//
//             return NoContent();
//         }
//
//         // GET api/<UserController>/5
//         [HttpGet("GetUserForUpdate/{userId}")]
//         public async Task<ActionResult<Result<GetUserForUpdateDto>>> GetUserForUpdate(string userId, string? password)
//         {
//             var command = await _mediator.Send(new GetUserForUpdateRequest { UserId = userId, Password = password });
//
//             if (command.IsSuccess)
//             {
//                 _userWithRolesLogger.LogDebug($"LogDebug ================ Профиль пользователя успешно получен: {userId}");
//                 return Ok(command);
//             }
//
//             _userWithRolesLogger.LogError($"LogDebugError ================ Получить профиль пользователя не удалось: {userId}");
//             foreach (var error in command.ValidationErrors!)
//             {
//                 return BadRequest(error);
//             }
//
//             return NoContent();
//         }
//
//         [HttpPost("CreateUser")]
//         [Authorize(Roles = RoleConst.Administrator)]
//         public async Task<ActionResult<Result<string>>> CreateUser([FromBody] CreateUserDto createUserDto)
//         {
//             var command = await _mediator.Send(new CreateUserRequest { CreateUser = createUserDto });
//
//             if (command.IsSuccess)
//             {
//                 _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно создан: {createUserDto.UserName}");
//                 return Ok(command);
//             }
//
//             _userWithRolesLogger.LogError($"LogDebugError ================ Удалить пользователея не удалось: {createUserDto.UserName}");
//             foreach (var error in command.ValidationErrors!)
//             {
//                 return BadRequest(error);
//             }
//
//             return NoContent();
//         }
//
//         // PUT api/<UserController>/5
//         [HttpPut("{userId}")]
//         [Authorize(Roles = RoleConst.Administrator)]
//         public async Task<ActionResult<Result<Unit>>> Put(string userId, [FromBody] UpdateUserDto updateUser)
//         {
//             if (userId == updateUser.Id)
//             {
//                 var command = await _mediator.Send(new UpdateUserRequest { UpdateUser = updateUser });
//
//                 if (command.IsSuccess)
//                 {
//                     _userWithRolesLogger.LogDebug($"LogDebug ================ Пользователь успешно обновлен: {updateUser.Id}");
//                     return Ok(command);
//                 }
//
//                 _userWithRolesLogger.LogError($"LogDebugError ================ Обновить пользователеля не удалось: {updateUser.Id}");
//                 foreach (var error in command.ValidationErrors!)
//                 {
//                     return BadRequest(error);
//                 }
//
//                 return NoContent();
//             }
//
//             return NoContent();
//         }
//
//         // PUT api/<UserController>/5
//         [Authorize]
//         [HttpPut("UpdateUserProfile/{userId}")]
//         public async Task<ActionResult<Result<GetUserForUpdateDto>>> UpdateUserProfile(string userId, [FromForm] UpdateUserProfileDto updateUserProfile)
//         {
//             if (userId == updateUserProfile.Id)
//             {
//                 var command = await _mediator.Send(new UpdateUserProfileRequest { UpdateUserProfile = updateUserProfile });
//
//                 if (command.IsSuccess)
//                 {
//                     _userWithRolesLogger.LogDebug($"LogDebug ================ Профиль пользователя успешно обновлен: {updateUserProfile.Id}");
//                     return Ok(command);
//                 }
//
//                 _userWithRolesLogger.LogError($"LogDebugError ================ Обновить профиль пользователеля не удалось: {updateUserProfile.Id}");
//                 foreach (var error in command.ValidationErrors!)
//                 {
//                     return BadRequest(error);
//                 }
//
//                 return NoContent();
//             }
//
//             else
//             {
//                 return NoContent();
//             }
//         }
//
//         // DELETE api/<UserController>/5
//         [HttpDelete("{userId}")]
//         [Authorize(Roles = RoleConst.Administrator)]
//         public async Task<ActionResult<Result<Unit>>> Delete(string userId, [FromBody] DeleteUserDto deleteUserDto)
//         {
//             if (userId == deleteUserDto.Id)
//             {
//                 var command = await _mediator.Send(new DeleteUserRequest { DeleteUser = deleteUserDto });
//
//                 if (command.IsSuccess)
//                 {
//                     _deleteLogger.LogDebug($"LogDebug ================ Пользователя успешно удален: {deleteUserDto.Id}");
//                     return Ok(command);
//                 }
//
//                 _deleteLogger.LogError($"LogDebugError ================ Удалить пользователеля не удалось: {deleteUserDto.Id}");
//                 foreach (var error in command.ValidationErrors!)
//                 {
//                     return BadRequest(error);
//                 }
//
//                 return NoContent();
//             }
//
//             else
//             {
//                 return NoContent();
//             }
//         }
//
//         [HttpDelete()]
//         [Authorize(Roles = RoleConst.Administrator)]
//         public async Task<ActionResult<Result<bool>>> Delete([FromBody] DeleteUsersDto deleteUsersDto)
//         {
//             var command = await _mediator.Send(new DeleteUsersRequest { DeleteUsers = deleteUsersDto });
//
//             if (command.IsSuccess)
//             {
//                 _boolLogger.LogDebug($"LogDebug ================ Пользователи успешно удалены: {deleteUsersDto.UserIds}");
//                 return Ok(command);
//             }
//
//             _deleteLogger.LogError($"LogDebugError ================ Удалить пользователелей не удалось: {deleteUsersDto.UserIds}");
//             foreach (var error in command.ValidationErrors!)
//             {
//                 return BadRequest(error);
//             }
//
//             return NoContent();
//         }
//     }
// }