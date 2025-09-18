using System.Security.Cryptography;
using System.Text;

namespace Services.Services
{
    public static class GlobalServices
    {
        public static String createControlCode()
        {
            Random random = new Random();
            int preCode = random.Next(1000, 9999);
            string lAnsObj = preCode.ToString();
            return lAnsObj;
        }        

    }
}
