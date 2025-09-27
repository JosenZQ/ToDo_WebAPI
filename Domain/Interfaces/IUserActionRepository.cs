using Infrastructure.Entities;

namespace Domain.Interfaces
{
    public interface IUserActionRepository
    {
        Task<List<UserAction>> getUserActionsByActionCode(string pActionCode);
        Task<List<UserAction>> getUserActionsByUserCode(string pUserCode);
        Task<bool> recordNewUserAction(UserAction pUserAction);
    }
}
