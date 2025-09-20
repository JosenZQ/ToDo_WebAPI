using Domain.Models;
using Infrastructure.Entities;

namespace Domain.Interfaces
{
    public interface IUserService
    {
        Task<User> getUserByCodeAsync(string pUserCode);
        Task<string> createNewUserAsync(UserCreateRequest pNewUser);
    }
}
