using MailKit.Net.Smtp;
using MimeKit;
using ProOffice_BookResources.EmailService.Interface;

namespace ProOffice_BookResources.EmailService
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfig;
        public EmailSender(EmailConfiguration emailConfig)
        {
            _emailConfig = emailConfig;
        }
        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        public async Task SendEmailAsync(Message message)
        {
            var mailMessage = CreateEmailMessage(message);
            await SendAsync(mailMessage);
        }


        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Viktor Stojkov - Junior .NET Developer", "viktor.stojkov93@gmail.com"));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }
        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.UserName, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch
                {
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }

        private async Task SendAsync(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 465, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    await client.AuthenticateAsync("viktor.stojkov93@gmail.com", "rgopqporrucvwqvx"); // email, password
                    await client.SendAsync(mailMessage);

                    //Линк до Google Account за да генерира нов пасворд за постоечкиот маил од кој што сакаме да го пратиме маилот
                    //https://accounts.google.com/signin/v2/challenge/pwd?TL=ALbfvL3IC6XvYTE8Z4dVYE3udF15qRJ3CozreV5Y-KhwrdLJ5D7ZHahHThjz2T-x&cid=2&continue=https%3A%2F%2Fmyaccount.google.com%2Fapppasswords%3Frapt%3DAEjHL4OlAufY6_2zGtKcEpNNwelsRxHEp62T6UpnkH2KRmPmG19uiTAUvuIO2Q-fka1hdgZcveAJqIYNcaZxE0I_JhFlwCWoew&flowName=GlifWebSignIn&ifkv=AWnogHfevlIUa8M3wHL40zwcNrYawDA01hBa-6g_EkuDSasJbGg3rgv-O12-SZkhPO30w_os4szK&rart=ANgoxcdD5Hx5cVy5SnTlfnCAgBZGBHDqbz-IOOuyTRcvTtKG9bqMTwBDKsYTwmW4lxNbQQSy2I0BKjk9jTThBjszpabmEvpRpw&sarp=1&scc=1&service=accountsettings&flowEntry=ServiceLogin

                    //Ги следев чекорите од следниот линк за Маил
                    //https://code-maze.com/aspnetcore-send-email/
                }
                catch
                {
                    //log an error message or throw an exception, or both.
                    throw;
                }
                finally
                {
                    await client.DisconnectAsync(true);
                    client.Dispose();
                }
            }
        }
    }
}
