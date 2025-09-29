using Infrastructure.Interfaces;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserTaskRepository : IUserTaskRepository
    {
        private readonly TaskListDbContext gDbContext;

        public UserTaskRepository(TaskListDbContext pDbContext)
        {
            gDbContext = pDbContext;
        }

        public async Task<List<UserTask>> getUserTasksList(string pUserCode)
        {
            return await gDbContext.UserTasks.Where(x => x.UserCode == pUserCode).ToListAsync();
        }

        public async Task<bool> saveNewUserTask(UserTask pNewUserTask)
        {
            gDbContext.UserTasks.Add(pNewUserTask);
            await gDbContext.SaveChangesAsync();
            return true;
        }

        public async Task<bool> updateUserTask(UserTask pUpdatedUserTask)
        {
            var lUserTaskFound = gDbContext.UserTasks.FirstOrDefault(x => x.TaskId == pUpdatedUserTask.TaskId);
            if (lUserTaskFound != null)
            {
                lUserTaskFound.Title = pUpdatedUserTask.Title;
                lUserTaskFound.Description = pUpdatedUserTask.Description;
                lUserTaskFound.CategoryCode = pUpdatedUserTask.CategoryCode;
                lUserTaskFound.State = pUpdatedUserTask.State;
                await gDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> deleteUserTask(UserTask pUserTask)
        {
            var lUserTaskFound = gDbContext.UserTasks.FirstOrDefault(x => x.TaskId == pUserTask.TaskId);
            if (lUserTaskFound != null)
            {
                gDbContext.UserTasks.Remove(lUserTaskFound);
                await gDbContext.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}
