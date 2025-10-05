using Domain.DTOs;
using Microsoft.Extensions.Configuration;
using Services.Interfaces;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace Services.Services
{
    public class GlobalServices : IGlobalServices
    {
        const int keySize = 64;
        const int iterations = 350000;        
        public static byte[] salt;
        HashAlgorithmName mHashAlgorithm = HashAlgorithmName.SHA512;
        private readonly HttpClient gHttpClient;
        private readonly IConfiguration gConfig;

        public GlobalServices(HttpClient pHttpClient, IConfiguration pConfig)
        {
            gHttpClient = pHttpClient;
            gConfig = pConfig;
        }

        public String createControlCode()
        {
            Random random = new Random();
            int preCode = random.Next(1000, 9999);
            string lAnsObj = preCode.ToString();
            return lAnsObj;
        }

        public String createVerificationCode()
        {
            Random random = new Random();
            int preCode = random.Next(100000, 999999);
            string lAnsObj = preCode.ToString();
            return lAnsObj;
        }

        public bool VerifyCodeLifeTime(DateTime pCreationTime, double pMinutesLifeTime)
        {
            double elapsedMinutes = (DateTime.Now - pCreationTime).TotalMinutes;
            return pMinutesLifeTime >= elapsedMinutes;
        }

        public String hashPassword(string pPassword, out byte[] salt)
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

        public bool verifyPassword(string pPassword, string pHashedPass, string pSalt)
        {
            byte[] saltBytes = Convert.FromHexString(pSalt);
            var hashToCompare = Rfc2898DeriveBytes.Pbkdf2(pPassword, saltBytes, iterations, mHashAlgorithm, keySize);
            return CryptographicOperations.FixedTimeEquals(hashToCompare, Convert.FromHexString(pHashedPass));
        }

        public async Task<GeoIpResponse> GetLocationByIpAddress(string pIpAddress)
        {
            try
            {
                string lApiKey = gConfig.GetSection("IpGeolocation:ApiKey").Value;
                var url = $"https://api.ipgeolocation.io/ipgeo?apiKey={lApiKey}&ip={pIpAddress}";

                var response = await gHttpClient.GetAsync(url);

                if (!response.IsSuccessStatusCode)
                    return null;

                var content = await response.Content.ReadAsStringAsync();
                Console.WriteLine(content);
                return JsonSerializer.Deserialize<GeoIpResponse>(content);
            }
            catch(Exception lEx)
            {
                throw lEx;
            }
        }
    }
}
