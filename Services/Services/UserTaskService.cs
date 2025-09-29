using Domain.DTOs;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Services.Interfaces;

namespace Services.Services
{
    public class UserTaskService : IUserTaskService
    {
        private readonly IUserTaskRepository gUserTaskRepo;

        public UserTaskService(IUserTaskRepository pUserTaskRepo)
        {
            gUserTaskRepo = pUserTaskRepo;
        }

        public async Task<List<UserTask>> GetUserTask(string pUserCode)
        {
            try
            {
                return await gUserTaskRepo.getUserTasksList(pUserCode);
            }
            catch(Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<bool> CreateNewUserTask(UserTaskFormat pNewUserTask)
        {
            try
            {
                if(pNewUserTask != null)
                {
                    UserTask lNewUserTask = new UserTask
                    {
                        UserCode = pNewUserTask.UserCode,
                        CategoryCode = pNewUserTask.CategoryCode,
                        Title = pNewUserTask.Title,
                        Description = pNewUserTask.Description,
                        DateCreated = pNewUserTask.DateCreated,
                        Deadline = pNewUserTask.Deadline,
                        State = pNewUserTask.State
                    };
                    return await gUserTaskRepo.saveNewUserTask(lNewUserTask);
                }
                else
                {
                    return false;
                }
            }
            catch(Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<bool> UpdateUserTask(UserTask pModifiedTask)
        {
            try
            {
                if (pModifiedTask != null)
                {                    
                    return await gUserTaskRepo.updateUserTask(pModifiedTask);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<bool> DeleteUserTask(UserTask pUserTask)
        {
            try
            {
                if (pUserTask != null)
                {
                    return await gUserTaskRepo.deleteUserTask(pUserTask);
                }
                else
                {
                    return false;
                }
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }
    }
}
