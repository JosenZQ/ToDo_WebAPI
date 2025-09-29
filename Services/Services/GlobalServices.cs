using Services.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace Services.Services
{
    public class GlobalServices : IGlobalServices
    {
        const int keySize = 64;
        const int iterations = 350000;
        public static byte[] salt;
        HashAlgorithmName mHashAlgorithm = HashAlgorithmName.SHA512;

        public String createControlCode()
        {
            Random random = new Random();
            int preCode = random.Next(1000, 9999);
            string lAnsObj = preCode.ToString();
            return lAnsObj;
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
    }
}
