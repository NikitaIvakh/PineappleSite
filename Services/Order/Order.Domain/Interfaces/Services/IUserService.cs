using Order.Domain.DTOs;
using Order.Domain.ResultOrder;

namespace Order.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserWithRolesDto>> GetUserAsync(string userId);
    }
}