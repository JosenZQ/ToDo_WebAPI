using Infrastructure.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserRepository
    {
        Task<User> getUserByCodeAsync(string pUserCode);
        Task<User> getUserByUsernameAsync(string pUsername);
        Task<User> getUserByEmailAsync(string pUsername);
        Task createNewUserAsync(User pNewUser);
        Task updateUserData(User pUser);
    }
}
