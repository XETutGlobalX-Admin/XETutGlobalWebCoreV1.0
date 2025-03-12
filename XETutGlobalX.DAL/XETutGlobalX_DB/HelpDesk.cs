using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace XETutGlobalX.DAL.XETutGlobalX_DB
{
    public class HelpDesk
    {
        public string SendMail(string from_Email, string from_Password, string toEmail, string subject, string body)
        {
            bool isMailSend = true;
            string SendMessage = string.Empty;
            string fromEmail = from_Email; // Replace with your email address
            string fromPassword = from_Password; // Replace with your email password


            MailMessage mail = new MailMessage();
            mail.From = new MailAddress(fromEmail);
            mail.To.Add(toEmail);

            mail.Subject = subject;
            mail.Body = body;

            SmtpClient smtpClient = new SmtpClient("smtp.gmail.com") // Replace with your SMTP server
            {
                Port = 587,
                Credentials = new NetworkCredential(fromEmail, fromPassword),
                EnableSsl = true,
            };

            try
            {
                smtpClient.Send(mail);
                isMailSend = true;
                SendMessage = $"Send email Successfully";
            }
            catch (Exception ex)
            {
                isMailSend = false;
                SendMessage = $"Failed to send email: {ex.Message}";
            }
            string jsonResponse = JsonSerializer.Serialize(new { Status = isMailSend, Message = SendMessage });
            return jsonResponse;
            //return "'SendOTPMailResponse': [{'Status':" + isMailSend.ToString() + "," + "'Message': "+ SendMessage + "}]}";
        }
    }
}
