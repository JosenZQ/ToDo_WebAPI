using Domain.Interfaces;
using Domain.Models;
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
            return await gUserService.getUserByCodeAsync(pUserCode);
        }

        [HttpPost("CreateNewUser")]
        public async Task<string> CreateNewUser(UserCreateRequest pNewUser)
        {
            return await gUserService.createNewUserAsync(pNewUser);
        }        
    }
}
