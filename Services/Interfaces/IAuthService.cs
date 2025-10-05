using Domain.DTOs;
using Domain.Models;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LogIn(string pEmail, string pPassword);
        Task<string> CreateNewUser(UserCreateRequest pNewUser);
        Task<string> RequestVerificationCode(VerificationCodeRequest pRequest);
        Task<string> VerifyCode(VerifyCodeRequest pRequest);
    }
}
