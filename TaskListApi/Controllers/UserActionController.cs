using Domain.DTOs;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace TaskListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserActionController : ControllerBase
    {
        private readonly IUserActionService gUserActionService;

        public UserActionController(IUserActionService pUserActionService)
        {
            gUserActionService = pUserActionService;
        }

        [HttpGet("GetUserActionsByUserCode")]
        public async Task<IEnumerable<UserAction>> GetUserActionsByUserCode(string pUserCode)
        {
            return await gUserActionService.GetUserActionsByUserCode(pUserCode);
        }

        [HttpGet("GetUserActionsByActionCode")]
        public async Task<IEnumerable<UserAction>> GetUserActionsByActionCode(string pActionCode)
        {
            return await gUserActionService.GetUserActionsByActionCode(pActionCode);
        }

        [HttpPost("SaveUserAction")]
        public async Task<bool> SaveUserAction(UserActionFormat pNewUserAction)
        {
            return await gUserActionService.SaveNewUserAction(pNewUserAction);
        }
    }
}
