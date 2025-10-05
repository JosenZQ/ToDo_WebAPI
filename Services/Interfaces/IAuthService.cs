using Domain.DTOs;
using Domain.Models;

namespace Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> LogIn(LogInRequest pRequest);
        Task<string> CreateNewUser(UserCreateRequest pNewUser);
        Task<string> RequestVerificationCode(VerificationCodeRequest pRequest);
        Task<string> VerifyCode(VerifyCodeRequest pRequest);
        Task<string> PasswordRecovery(PassResetRequest pRequest);
    }
}
