using Infrastructure.Interfaces;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class UserActionsRepository : IUserActionRepository
    {
        private readonly TaskListDbContext gDbContext;

        public UserActionsRepository(TaskListDbContext pDbContext)
        {
            gDbContext = pDbContext;
        }

        public async Task<List<UserAction>> getUserActionsByActionCode(string pActionCode)
        {
            return await gDbContext.UserActions.Where(x => x.ActionCode == pActionCode).ToListAsync();
        }

        public async Task<List<UserAction>> getUserActionsByUserCode(string pUserCode)
        {
            return await gDbContext.UserActions.Where(x => x.UserCode == pUserCode).ToListAsync();
        }

        public async Task<bool> recordNewUserAction(UserAction pUserAction)
        {
            gDbContext.UserActions.Add(pUserAction);
            await gDbContext.SaveChangesAsync();
            return true;
        }
    }
}