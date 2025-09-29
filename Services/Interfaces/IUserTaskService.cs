using Domain.DTOs;
using Infrastructure.Entities;

namespace Services.Interfaces
{
    public interface IUserTaskService
    {
        Task<List<UserTask>> GetUserTask(string pUserCode);
        Task<bool> CreateNewUserTask(UserTaskFormat pNewUserTask);
        Task<bool> UpdateUserTask(UserTask pModifiedTask);
        Task<bool> DeleteUserTask(UserTask pUserTask);
    }
}
