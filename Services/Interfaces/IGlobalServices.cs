using Domain.DTOs;

namespace Services.Interfaces
{
    public interface IGlobalServices
    {
        String createControlCode();
        String createVerificationCode();
        bool VerifyCodeLifeTime(DateTime pCreationTime, double pMinutesLifeTime);
        String hashPassword(string pPassword, out byte[] salt);
        bool verifyPassword(string pPassword, string pHashedPass, string pSalt);
        Task<GeoIpResponse> GetLocationByIpAddress(string pIpAddress);
    }
}
