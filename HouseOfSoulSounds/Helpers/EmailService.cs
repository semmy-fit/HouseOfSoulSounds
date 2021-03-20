using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
namespace HouseOfSoulSounds.Helpers
{
    public class EmailService
    {
        public static async Task<string> SendEmailAsync(string name, string email, string sendEmail, string subject, string msg)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress($"Администрация сайта - {Config.Name}", sendEmail));
            emailMessage.To.Add(new MailboxAddress(name, email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = msg
            };

            using var client = new SmtpClient();

            await client.ConnectAsync("smtp.gmail.com", 587, false);
            if (!client.IsConnected)
                return "Нет соединения с сервером!";
            try
            {
                await client.AuthenticateAsync(Config.Email, Config.EmailPass);
            }
            catch
            {
                return "Север не отвечает!";
            }

            if (!client.IsAuthenticated)
                return "Север не отвечает!";
            try
            {
                await client.SendAsync(emailMessage);
            }
            catch
            {
                return "Нет возможности выслать подтверждение!";
            }
            await client.DisconnectAsync(true);
            return string.Empty;
        }

        /*public static  string SendEmailAsync(string name, string email, string sendEmail, string subject, string msg)
        {
            System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
            message.From = new MailAddress(sendEmail, $"Администрация сайта - {Config.Name}");
            message.To.Add(email); //адресат сообщения
            message.Subject = subject; //тема сообщения
            message.IsBodyHtml = true;
            System.Text.Encoding encoding= System.Text.Encoding.UTF8;
            message.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = msg
            }.GetText(encoding);

            //var emailMessage = new MimeMessage();

            //emailMessage.From.Add(new MailboxAddress($"Администрация сайта - {Config.Name}", sendEmail));
            //emailMessage.To.Add(new MailboxAddress(name, email));
            //emailMessage.Subject = subject;
            //emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            //{
            //    Text = msg
            //};

            using (var client = new System.Net.Mail.SmtpClient("smtp.gmail.com"))
            {

           
                client.Credentials = new NetworkCredential(Config.Email,Config.EmailPass); //логин-пароль от аккаунта
            client.Port = 587; //порт 587 либо 465
            client.EnableSsl = true; //SSL обязательно


            try
            {
                client.SendAsync(message, null);
            }
            catch
            {
                return "Нет возможности выслать подтверждение!";
            }

            }
            //await client.ConnectAsync("smtp.gmail.com", 587, false);
            //if (!client.IsConnected)
            //    return "Нет соединения с сервером!";
            //try
            //{
            //    await client.AuthenticateAsync(Config.Email, Config.EmailPass);
            //}
            //catch
            //{
            //    return "Север не отвечает!";
            //}

            //if (!client.IsAuthenticated)
            //    return "Север не отвечает!";
            //try
            //{
            //    await client.SendAsync(emailMessage);
            //}
            //catch
            //{
            //    return "Нет возможности выслать подтверждение!";
            //}
            //await client.DisconnectAsync(true);
            return string.Empty;
        }*/
    }
}