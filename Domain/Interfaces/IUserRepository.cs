using Infrastructure.Entities;

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
