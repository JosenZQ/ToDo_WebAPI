using Infrastructure.Entities;

namespace Domain.Interfaces
{
    public interface IUserTaskRepository
    {
        Task<List<UserTask>> getUserTasksList(string pUserCode);
        Task<bool> saveNewUserTask(UserTask pNewUserTask);
        Task<bool> updateUserTask(UserTask pUpdatedUserTask);
        Task<bool> deleteUserTask(UserTask pUserTask);
    }
}
