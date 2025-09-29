namespace Services.Interfaces
{
    public interface IGlobalServices
    {
        String createControlCode();
        String hashPassword(string pPassword, out byte[] salt);
        bool verifyPassword(string pPassword, string pHashedPass, string pSalt);
    }
}
