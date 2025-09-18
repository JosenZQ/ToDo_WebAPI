using Infrastructure.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> getUserByCodeAsync(string pUserCode);
        Task<User> getUserByUsernameAsync(string pUsername);
        Task createNewUserAsync(User pNewUser);
        Task updateUserData(User pUser);
    }
}
