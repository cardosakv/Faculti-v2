using System;
using System.Data;
using System.Net.Mail;
using System.Windows;

namespace Faculti.Services
{
    internal class Email
    {
        /// <summary>
        ///     Sends verification code to the specified recepient email using Faculti credentials.
        /// </summary>
        public static void SendVerificationCode(string recepientEmail, int code)
        {
            try
            {
                MailMessage mail = new();
                SmtpClient SmtpServer = new("smtp.gmail.com");

                mail.From = new MailAddress("info.faculti@gmail.com");
                mail.To.Add(recepientEmail);
                mail.Subject = "Faculti Confirmation Code";
                mail.Body = "Your Faculti account confirmation code is: " + code + ".";

                SmtpServer.Port = 587;
                SmtpServer.Credentials = new System.Net.NetworkCredential("info.faculti", "faculti1234");
                SmtpServer.EnableSsl = true;

                SmtpServer.Send(mail);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending verification code.\n" + ex);
            }
        }
    }
}