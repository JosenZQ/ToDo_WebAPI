using Domain.DTOs;
using Domain.Interfaces;
using Infrastructure.Entities;

namespace Services.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository gUserRepo;
        private readonly IGlobalServices gGlobalServices;
        private byte[] salt;

        public UserService(IUserRepository pUserRepo, IGlobalServices pGlobalServices)
        {
            gUserRepo = pUserRepo;
            gGlobalServices = pGlobalServices;
        }

        public async Task<User> getUserByCodeAsync(string pUserCode)
        {
            return await gUserRepo.getUserByCodeAsync(pUserCode);
        }

        public async Task<string> ChangePassword(PassChangeRequest pRequest)
        {
            try
            {
                User lUserFound = await gUserRepo.getUserByCodeAsync(pRequest.UserCode);
                if (pRequest.Password == pRequest.PassConfirm)
                {
                    string lHashPass = gGlobalServices.hashPassword(pRequest.PassConfirm, out salt);
                    var lSalt = Convert.ToHexString(salt);
                    lUserFound.Password = lHashPass;
                    lUserFound.Salt = lSalt;
                    await gUserRepo.updateUserData(lUserFound);
                    return "Contraseña actualizada correctamente";
                }
                else
                {
                    return "Las contraseñas no coinciden";
                }
            }
            catch (Exception lEx)
            {
                return lEx.Message;
            }
        }

    }
}
