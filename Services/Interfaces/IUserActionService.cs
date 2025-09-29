using Domain.DTOs;
using Infrastructure.Entities;

namespace Services.Interfaces
{
    public interface IUserActionService
    {
        Task<List<UserAction>> GetUserActionsByUserCode(string pUserCode);
        Task<List<UserAction>> GetUserActionsByActionCode(string pActionCode);
        Task<bool> SaveNewUserAction(UserActionFormat pNewUserAction);
    }
}
