using Domain.Interfaces;
using Infrastructure.Data;
using Infrastructure.Entities;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly TaskListDbContext gDbContext;

        public UserRepository(TaskListDbContext pDbContext)
        {
            gDbContext = pDbContext;
        }

        public async Task<User> getUserByCodeAsync(string pUserCode)
        {
            return await gDbContext.Users.FirstOrDefaultAsync(x => x.UserCode == pUserCode);
        } 

        public async Task<User> getUserByUsernameAsync(string pUsername)
        {
            return await gDbContext.Users.FirstOrDefaultAsync(x => x.UserName == pUsername);
        }

        public async Task<User> getUserByEmailAsync(string pUserEmail)
        {
            return await gDbContext.Users.FirstOrDefaultAsync(x => x.Email == pUserEmail);
        }

        public async Task createNewUserAsync(User pNewUser)
        {
            gDbContext.Users.Add(pNewUser);
            await gDbContext.SaveChangesAsync();
        }

        public async Task updateUserData(User pUser)
        {
            var lExistingUser = await gDbContext.Users.FirstOrDefaultAsync(x => x.UserCode == pUser.UserCode);
            if (lExistingUser != null)
            {
                lExistingUser.UserName = pUser.UserName;
                lExistingUser.Email = pUser.Email;
                lExistingUser.Password = pUser.Password;

                await gDbContext.SaveChangesAsync();
            }
            else
            {
                throw new InvalidOperationException("User not found.");
            }
        }


    }
}
