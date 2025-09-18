using Domain.Interfaces;

namespace Services.Services
{
    public class CodeService
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
