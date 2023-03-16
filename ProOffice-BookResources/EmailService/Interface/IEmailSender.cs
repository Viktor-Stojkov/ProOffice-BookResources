namespace ProOffice_BookResources.EmailService.Interface
{
    public interface IEmailSender
    {
        void SendEmail(Message message);
        Task SendEmailAsync(Message message);

    }
}
