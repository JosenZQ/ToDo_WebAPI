using Infrastructure.Entities;

namespace Infrastructure.Interfaces
{
    public interface IUserActionRepository
    {
        Task<List<UserAction>> getUserActionsByActionCode(string pActionCode);
        Task<List<UserAction>> getUserActionsByUserCode(string pUserCode);
        Task<bool> recordNewUserAction(UserAction pUserAction);
    }
}
