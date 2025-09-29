using Domain.DTOs;
using Domain.Interfaces;
using Infrastructure.Entities;
using Services.Interfaces;

namespace Services.Services
{
    public class UserActionService : IUserActionService
    {
        private readonly IUserActionRepository gUserActionRepo;

        public const string Access = "3722";
        public const string Insert = "8652";
        public const string Modify = "4753";
        public const string Delete = "9376";

        public UserActionService(IUserActionRepository pUserActionRepo)
        {
            gUserActionRepo = pUserActionRepo;
        }

        public async Task<List<UserAction>> GetUserActionsByUserCode(string pUserCode)
        {
            try
            {
                return await gUserActionRepo.getUserActionsByActionCode(pUserCode);                
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<List<UserAction>> GetUserActionsByActionCode(string pActionCode)
        {
            try
            {
                return await gUserActionRepo.getUserActionsByActionCode(pActionCode);
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<bool> SaveNewUserAction(UserActionFormat pNewUserAction)
        {
            try
            {
                if(pNewUserAction != null)
                {
                    UserAction lNewUserAction = new UserAction
                    {
                        UserCode = pNewUserAction.UserCode,
                        ActionCode = pNewUserAction.ActionCode,
                        ActionDescr = pNewUserAction.ActionDescr,
                        ActionDate = pNewUserAction.ActionDate                        
                    };
                    return await gUserActionRepo.recordNewUserAction(lNewUserAction);
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
