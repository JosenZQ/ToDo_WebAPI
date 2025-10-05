using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace TaskListApi.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthService gAuthService;

        public AuthenticationController(IAuthService pAuthService)
        {
            gAuthService = pAuthService;
        }

        [HttpPost("LogIn")]
        public async Task<string> LogIn(string pEmail, string pPassword)
        {
            return await gAuthService.LogIn(pEmail, pPassword);
        }

        [HttpPost("Register")]
        public async Task<string> Register(UserCreateRequest pNewUser)
        {
            return await gAuthService.CreateNewUser(pNewUser);
        }

        [HttpPost("RequestVerificationCode")]
        public async Task<string> RequestVerificationCode(VerificationCodeRequest pRequest)
        {
            return await gAuthService.RequestVerificationCode(pRequest);
        }

        [HttpPost("VerifyCode")]
        public async Task<string> VerifyCode(VerifyCodeRequest pRequest)
        {
            return await gAuthService.VerifyCode(pRequest);
        }

    }
}
