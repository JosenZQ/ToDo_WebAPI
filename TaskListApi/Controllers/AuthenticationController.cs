using Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        [HttpGet("LogIn")]
        public async Task<string> LogIn(string pEmail, string pPassword)
        {
            return await gAuthService.LogIn(pEmail, pPassword);
        }

    }
}
