using Domain.Models;

namespace Domain.Interfaces
{
    public interface IAuthService
    {
        Task<string> LogIn(string pEmail, string pPassword);
        Task<string> CreateNewUser(UserCreateRequest pNewUser);
    }
}
