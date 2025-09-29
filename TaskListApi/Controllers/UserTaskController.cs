using Domain.DTOs;
using Infrastructure.Entities;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace TaskListApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserTaskController : ControllerBase
    {
        private readonly IUserTaskService gUserTaskService;

        public UserTaskController(IUserTaskService pUserTaskService)
        {
            gUserTaskService = pUserTaskService;
        }

        [HttpGet("GetUserTasks")]
        public async Task<IEnumerable<UserTask>> GetUserTasks(string pUserCode)
        {
            return await gUserTaskService.GetUserTask(pUserCode);
        }

        [HttpPost("CreateNewUserTask")]
        public async Task<bool> CreateNewUserTask(UserTaskFormat pNewUserTask)
        {
            return await gUserTaskService.CreateNewUserTask(pNewUserTask);
        }

        [HttpPut("UpdateUserTask")]
        public async Task<bool> UpdateUserTask(UserTask pModifiedTask)
        {
            return await gUserTaskService.UpdateUserTask(pModifiedTask);
        }

        [HttpDelete("DeleteUserTask")]
        public async Task<bool> DeleteUserTask(UserTask pUserTask)
        {
            return await gUserTaskService.DeleteUserTask(pUserTask);
        }

    }
}
