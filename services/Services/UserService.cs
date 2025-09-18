using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Entities;
using System.Text.Json.Nodes;
using System.Timers;

namespace Services.Services
{
    public class UserService
    {
        private readonly IUserRepository gUserRepo;
        private CodeService codeService = new CodeService();

        public UserService(IUserRepository pUserRepo)
        {
            gUserRepo = pUserRepo;
        }

        public async Task<User> getUserByCodeAsync(string pUserCode)
        {
            return await gUserRepo.getUserByCodeAsync(pUserCode);
        }

        public async Task<string> createNewUserAsync(UserCreateRequest pNewUser)
        {
            try
            {
                //Verificar que no exista un email duplicado:
                var lUserConfirm = gUserRepo.getUserByEmailAsync(pNewUser.Email);
                if (lUserConfirm == null)
                {
                    //Crear un código único para cada nuevo usuario:
                    string lNewCode;
                    while (true)
                    {
                        lNewCode = CodeService.createControlCode();
                        var lUser = gUserRepo.getUserByCodeAsync(lNewCode);
                        if (lUser == null) break;
                    }

                    //Verificar si las contraseñas coinciden:
                    if (pNewUser.Password == pNewUser.PasswordConfirm)
                    {
                        User _NewUser = new User
                        {
                            UserCode = lNewCode,
                            UserName = pNewUser.UserName,
                            Email = pNewUser.Email,
                            Password = pNewUser.PasswordConfirm
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
