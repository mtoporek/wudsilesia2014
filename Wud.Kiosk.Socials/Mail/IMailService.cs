namespace Wud.Kiosk.Socials.Mail
{
    public interface IMailService
    {
        void Configure(string mailFrom, int portNumber, string password, bool enableSSL, string smtpServer);

        void SendMail(Mail mail);
    }
}
