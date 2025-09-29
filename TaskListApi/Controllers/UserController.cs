using Domain.DTOs;
using Domain.Interfaces;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace TaskListApi.Controllers
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [ApiController]
    public class UserController : ControllerBase
    {
        public readonly IUserService gUserService;

        public UserController(IUserService pUserService)
        {
            gUserService = pUserService;
        }

        [HttpGet("GetUserByCode")]
        public async Task<User> GetUserByCode(string pUserCode)
        {
            return await gUserService.GetUserByCodeAsync(pUserCode);
        }

        [HttpPost("ChangePassword")]
        public async Task<string> ChangePassword(PassChangeRequest pRequest)
        {
            return await gUserService.ChangePassword(pRequest);
        }

    }
}
