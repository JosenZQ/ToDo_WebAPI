using Domain.Models;
using Infrastructure.Entities;
using Infrastructure.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Services.Interfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Services.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository gUserRepo;
        private readonly IGlobalServices gGlobalServices;
        private readonly IConfiguration gConfig;
        private byte[] salt;

        public AuthService(IUserRepository pUserRepo, IGlobalServices pGlobalServices, IConfiguration pConfig)
        {
            gUserRepo = pUserRepo;
            gGlobalServices = pGlobalServices;
            gConfig = pConfig;
        }

        public string generateJWT(User pUser)
        {
            try
            {
                /* GENERATE CLAIMS WITH USER DATA */
                var lUserClaims = new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, pUser.UserCode),
                    new Claim(ClaimTypes.Name, pUser.UserName),
                    new Claim(ClaimTypes.Email, pUser.Email),
                    new Claim(ClaimTypes.MobilePhone, pUser.Phone)
                };

                /* CREATE THE SEURITY KEY WITH THE ONE IN THE appsettings.json TO THEN CREATE
                 * THE CREDENTIALS TO GENERATE THE JWT */
                var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(gConfig["Jwt:key"]!));
                var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256Signature);

                /* SET UP THE JWT CONFIGURATIONS BEFORE GENERATE IT */
                var jwtConfig = new JwtSecurityToken(
                    claims: lUserClaims,
                    expires: DateTime.UtcNow.AddMinutes(15),
                    signingCredentials: credentials
                );

                /* GENERATE THE JWT AND RETURN IT */
                return new JwtSecurityTokenHandler().WriteToken(jwtConfig);
            }
            catch (Exception lEx) 
            {
                throw lEx;
            }
        }

        public async Task<string> LogIn(string pEmail, string pPassword)
        {
            try
            {
                string lErrorMessage = "Usuario y/o contraseña incorrectos";
                if (pEmail != "" && pPassword != "")
                {
                    User lUser = await gUserRepo.getUserByUsernameAsync(pEmail);
                    if (lUser != null)
                    {
                        bool lVerify = gGlobalServices.verifyPassword(pPassword, lUser.Password, lUser.Salt);
                        if (lVerify == true)
                        {
                            string lApiToken = generateJWT(lUser);
                            return lApiToken;
                        }
                        else
                        {
                            return lErrorMessage;
                        }
                    }
                    else
                    {
                        return lErrorMessage;
                    }
                }
                else
                {
                    return lErrorMessage;
                }
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<string> CreateNewUser(UserCreateRequest pNewUser)
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
                        var lHashedPass = gGlobalServices.hashPassword(pNewUser.PasswordConfirm!, out salt);
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
