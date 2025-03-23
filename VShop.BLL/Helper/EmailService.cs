using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace VShop.BLL.Helper
{
    public class EmailService
    {
        public static async Task SendMailAsync(string name, string subject, string content, string toMail)
        {
            try
            {
                using (var message = new MailMessage())
                using (var smtp = new SmtpClient())
                {
                    smtp.Host = "sandbox.smtp.mailtrap.io";
                    smtp.Port = 2525;
                    smtp.EnableSsl = true;
                    smtp.Credentials = new NetworkCredential("5b7bc0110d7864", "84a956a842cd0c");
                    smtp.Timeout = 20000;

                    message.From = new MailAddress("vShop@gmail.com", name);
                    message.To.Add(toMail);
                    message.Subject = subject;
                    message.IsBodyHtml = true;
                    message.Body = content;

                    await smtp.SendMailAsync(message); // Gửi email bất đồng bộ
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: send Email " + ex.Message);
            }
        }


    }
}
