using Domain.Interfaces;
using Domain.Models;
using Infrastructure.Entities;
using System.Security.Cryptography;
using System.Text;

namespace Services.Services
{
    public class UserService : iUserService
    {
        private readonly IUserRepository gUserRepo;

        const int keySize = 64;
        const int iterations = 350000;
        public byte[] salt;
        HashAlgorithmName mHashAlgorithm = HashAlgorithmName.SHA512;

        public UserService(IUserRepository pUserRepo)
        {
            gUserRepo = pUserRepo;
        }

        /* HASH PASSWORD FUNCTION (MADE GLOBALY TO ACCESS IT LATER)*/
        private String hashPassword(string pPassword, out byte[] salt)
        {
            salt = RandomNumberGenerator.GetBytes(keySize);

            var lHash = Rfc2898DeriveBytes.Pbkdf2(
                Encoding.UTF8.GetBytes(pPassword),
                salt,
                iterations,
                mHashAlgorithm,
                keySize
            );
            return Convert.ToHexString(lHash);
        }

        /* VERIFY PASSWORD */
        private bool verifyPassword(string pPassword, string pHashedPass, string pSalt)
        {
            byte[] saltBytes = Convert.FromHexString(pSalt);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(pPassword, saltBytes, iterations, mHashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(pHashedPass));
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
                        lNewCode = GlobalServices.createControlCode();
                        User lUser = await gUserRepo.getUserByCodeAsync(lNewCode);
                        if (lUser == null)
                        {
                            break;
                        }
                    }

                    //VERIFY BOTH PASSWORDS:
                    if (pNewUser.Password == pNewUser.PasswordConfirm)
                    {
                        var lHashedPass = hashPassword(pNewUser.PasswordConfirm, out salt);
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

        public async Task<bool> LogIn(string pEmail, string pPassword)
        {
            try
            {
                if (pEmail != "" && pPassword != "")
                {
                    User lUser = await gUserRepo.getUserByUsernameAsync(pEmail);
                    if (lUser != null) 
                    {
                        return verifyPassword(pPassword, lUser.Password, lUser.Salt);
                    }
                    else
                    {
                        return false;
                    }
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

        //public async Task<string> updateUserName(string pUserCode, string pNewUserName)
        //{
        //    try
        //    {
        //        var lUserConfirm = gUserRepo.getUserByCodeAsync(pUserCode);


        //    }
        //    catch (Exception lEx)
        //    {
        //        return $"Error: {lEx.Message}";
        //    }

        //}


    }
}
