using Domain.DTOs;
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
        private readonly IEmailService gEmailService;
        private readonly IEmailContentService gEmailContentService;
        private readonly IConfiguration gConfig;
        private static List<UserLocalVerificationCode> gVerificationCodes = new List<UserLocalVerificationCode>();
        private static List<UserWithVerificationCode> gUsersWithCodeList = new List<UserWithVerificationCode>();
        private byte[] salt;

        public AuthService(IUserRepository pUserRepo, IGlobalServices pGlobalServices,
            IConfiguration pConfig, IEmailService pEmailService, IEmailContentService pEmailContentService)
        {
            gUserRepo = pUserRepo;
            gGlobalServices = pGlobalServices;
            gConfig = pConfig;
            gEmailService = pEmailService;
            gEmailContentService = pEmailContentService;
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

        private UserWithVerificationCode VerifyExistingUser(string pUserCode)
        {
            UserWithVerificationCode lAnsObj = new UserWithVerificationCode();
            foreach (var item in gUsersWithCodeList)
            {
                if(item.UserCode == pUserCode)
                {
                    lAnsObj = item;
                    break;
                }
            }
            return lAnsObj;
        }

        private void ChangeVerifiedUserStatus(string pUserCode)
        {
            foreach (var item in gUsersWithCodeList)
            {
                if (item.UserCode == pUserCode)
                {
                    item.Verified = true;
                    break;
                }
            }
        }

        private void DeleteVerificationCodes(string pUserCode)
        {
            foreach(var item in gVerificationCodes)
            {
                if(item.UserCode == pUserCode)
                {
                    gVerificationCodes.Remove(item);
                    break;
                }
            }
        } 

        public async Task<string> LogIn(LogInRequest pRequest)
        {
            try
            {
                string lErrorMessage = "Usuario y/o contraseña incorrectos";
                if (pRequest.UserName != "" && pRequest.Password != "")
                {
                    User lUser = await gUserRepo.getUserByUsernameAsync(pRequest.UserName);
                    if (lUser != null)
                    {
                        bool lVerify = gGlobalServices.verifyPassword(pRequest.Password, lUser.Password, lUser.Salt);
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
                //VERIFY THE USERNAME ISN´T ALREADY ON DB:
                User lUserWithUserName = await gUserRepo.getUserByUsernameAsync(pNewUser.UserName);
                if(lUserWithUserName == null)
                {
                    //VERIFY THE EMAIL ISN´T ALREADY ON DB:
                    User lUserWithEmail = await gUserRepo.getUserByEmailAsync(pNewUser.Email);
                    if (lUserWithEmail == null)
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
                            GeoIpResponse lLocationDetails = await gGlobalServices.GetLocationByIpAddress(pNewUser.IpAddress);

                            RegisterEmailModel lModel = new RegisterEmailModel
                            {
                                UserName = _NewUser.UserName,
                                IPAddress = pNewUser.IpAddress,
                                Country = lLocationDetails.country_name,
                                State = lLocationDetails.state_prov,
                                City = lLocationDetails.city,
                                RegisterDate = DateTime.Now.ToString(),
                                TimeZoneName = lLocationDetails.TimeZone.Name,
                                TimeZoneCurrentTime = lLocationDetails.TimeZone.CurrentTime
                            };
                            string lEmailBodyContent = gEmailContentService.GetRegisterEmailBodyContent(lModel);
                            await gUserRepo.createNewUserAsync(_NewUser);
                            gEmailService.SendEmail(_NewUser.Email, "Notificación de Nuevo Registro", lEmailBodyContent);
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
                else
                {
                    return "Nombre de usuario ya registrado en el sistema";
                }
            }
            catch (Exception lEx)
            {
                return $"{lEx.Message}";
            }
        }

        public async Task<string> RequestVerificationCode(VerificationCodeRequest pRequest)
        {
            try
            {
                /* Verify if the user who request a verification code already exists on db */
                User lUserFound = await gUserRepo.getUserByUsernameAsync(pRequest.UserName);
                if (lUserFound != null)
                {
                    /* If exists, make a register that they requested a code and isn´t verified yet */
                    if (VerifyExistingUser(lUserFound.UserCode) != null)
                    {
                        UserWithVerificationCode lUserToVerify = new UserWithVerificationCode
                        {
                            UserCode = lUserFound.UserCode,
                            Verified = false
                        };
                        gUsersWithCodeList.Add(lUserToVerify);
                    }

                    /* Create a new verification code and give it to the user with the security parameters 
                     of the code and save it for verification */
                    string lVerificationCode = gGlobalServices.createVerificationCode();
                    UserLocalVerificationCode lUserVerificationCode = new UserLocalVerificationCode
                    {
                        UserCode = lUserFound.UserCode,
                        VerificationCode = lVerificationCode,
                        GenerationDateTime = DateTime.Now,
                        MinutesLifeTime = 5
                    };
                    gVerificationCodes.Add(lUserVerificationCode);

                    /* Make the verification model for the verification code email for the user */
                    VerificationCodeEmailModel lModel = new VerificationCodeEmailModel
                    {
                        UserName = lUserFound.UserName,
                        VerificationCode = lVerificationCode,
                        MinutesLifeTime = lUserVerificationCode.MinutesLifeTime.ToString()
                    };
                    string lEmailBodyContent = gEmailContentService.GetVerificationCodeEmailBodyContent(lModel);
                    gEmailService.SendEmail(lUserFound.Email, "Codigo de verificación", lEmailBodyContent);
                    return lUserFound.UserCode;
                }
                else
                {
                    return "Nombre de usuario o correo incorrecto";
                }
            }
            catch(Exception lEx)
            {
                throw lEx;
            }
        }

        public async Task<string> VerifyCode(VerifyCodeRequest pRequest)
        {
            try
            {
                if(pRequest != null)
                {
                    UserLocalVerificationCode lUserVerificationCode = new UserLocalVerificationCode();
                    foreach (var item in gVerificationCodes)
                    {
                        if (item.UserCode == pRequest.UserCode
                            && item.VerificationCode == pRequest.VerificationCode)
                        {
                            lUserVerificationCode = item;                            
                            break;
                        }
                    }

                    double lCodeLifeTime = Convert.ToDouble(lUserVerificationCode.MinutesLifeTime);
                    bool lCodeLifeTimeVerification = gGlobalServices.VerifyCodeLifeTime(lUserVerificationCode.GenerationDateTime, lCodeLifeTime);
                    if (lCodeLifeTimeVerification == true)
                    {
                        if (lUserVerificationCode.VerificationCode == pRequest.VerificationCode)
                        {
                            DeleteVerificationCodes(pRequest.UserCode);
                            ChangeVerifiedUserStatus(pRequest.UserCode);
                            return "Código verificado";
                        }
                        else
                        {
                            return "Código incorrecto";
                        }
                    }
                    else
                    {
                        return "El código de verificación ha expirado";
                    }

                }
                else
                {
                    return "Parámetros de consulta inválidos";
                }
            }
            catch (Exception lEx) 
            {
                throw lEx;
            }
        }

        public async Task<string> PasswordRecovery(PassResetRequest pRequest)
        {
            try
            {
                if(pRequest != null)
                {
                    var lUserWithCode = VerifyExistingUser(pRequest.UserCode);
                    if (lUserWithCode != null && lUserWithCode.Verified != false)
                    {
                        User lUserFound = await gUserRepo.getUserByCodeAsync(pRequest.UserCode);
                        if (pRequest.Password == pRequest.PassConfirm)
                        {
                            string lHashPass = gGlobalServices.hashPassword(pRequest.PassConfirm, out salt);
                            var lSalt = Convert.ToHexString(salt);
                            lUserFound.Password = lHashPass;
                            lUserFound.Salt = lSalt;
                            await gUserRepo.updateUserData(lUserFound);
                            gUsersWithCodeList.Remove(lUserWithCode);
                            return "Contraseña actualizada correctamente";
                        }
                        else
                        {
                            return "Las contraseñas no coinciden";
                        }
                    }
                    else
                    {
                        return "Se necesita primero estar verificado";
                    }
                }
                else
                {
                    return "Datos de solicitud inválidos";
                }
            }
            catch (Exception lEx)
            {
                throw lEx;
            }
        }

    }
}
