using Identity.Core.Entities.Identities;
using Identity.Core.Entities.User;
using Identity.Core.Entities.Users;
using Identity.Core.Interfaces;
using Identity.Core.Response;
using Microsoft.AspNetCore.Mvc;

namespace Identity.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController(IUserService userService) : ControllerBase
    {
        private readonly IUserService _userService = userService;

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<BaseIdentityResponse<IEnumerable<UserWithRoles>>>> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        // GET api/<UserController>/5
        [HttpGet("GetUserById/{id}")]
        public async Task<ActionResult<BaseIdentityResponse<ApplicationUser>>> GetUserById(string id)
        {
            var user = await _userService.GetByIdAsync(id);
            return Ok(user);
        }

        // POST api/<UserController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<UserController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<UserController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
