using Domain.DTOs;
using Infrastructure.Entities;

namespace Services.Interfaces
{
    public interface IUserService
    {
        Task<User> GetUserByCodeAsync(string pUserCode);
        Task<string> ChangePassword(PassResetRequest pRequest);
    }
}
