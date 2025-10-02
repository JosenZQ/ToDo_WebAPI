namespace Services.Interfaces
{
    public interface IEmailService
    {
        void SendEmail(string pReceiver, string pSubject, string pMessage);
    }
}
