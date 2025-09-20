using Domain.Interfaces;
using Domain.Models;
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

        public async Task<string> createNewUserAsync(UserCreateRequest pNewUser)
        {
            try
            {
                //VERIFY THE EMAIL ISN´T ALREADY ON DB:
                User lUserConfirm = await gUserRepo.getUserByUsernameAsync(pNewUser.Email);
                if (lUserConfirm == null)
                {
                    //GENERATE AN UNIQUE CODE FOR EACH NEW USER:
                    string lNewCode;
                    while (true)
                    {
                        lNewCode = gGlobalServices.createControlCode();
                        User lUser = await gUserRepo.getUserByCodeAsync(lNewCode);
                        if (lUser == null)
                        {
                            break;
                        }
                    }

                    //VERIFY BOTH PASSWORDS:
                    if (pNewUser.Password == pNewUser.PasswordConfirm)
                    {
                        var lHashedPass = gGlobalServices.hashPassword(pNewUser.PasswordConfirm, out salt);
                        var lSalt = Convert.ToHexString(salt);

                        User _NewUser = new User
                        {
                            UserCode = lNewCode,
                            UserName = pNewUser.UserName,
                            Email = pNewUser.Email,
                            Password = lHashedPass,
                            Phone = pNewUser.Phone,
                            Salt = lSalt
                        };
                        await gUserRepo.createNewUserAsync(_NewUser);
                        return "Usuario registrado correctamente";
                    }
                    else
                    {
                        return "Las contraseñas no coinciden";
                    }

                }
                else
                {
                    return "Correo ya registrado en el sistema";
                }
            }
            catch (Exception lEx)
            {
                return $"{lEx.Message}";
            }
        }
    }
}
