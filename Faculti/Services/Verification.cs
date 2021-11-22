using Faculti.Helpers;
using System;
using System.Data;
using System.Net.Mail;
using System.Threading.Tasks;
using System.Windows;

namespace Faculti.Services
{
    internal class Verification
    {
        /// <summary>
        /// Sends verification code to the specified recepient email using Faculti credentials.
        /// </summary>
        public static async Task SendCodeToEmailAsync(string recepientEmail, int code)
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

                await Task.Run(() => SmtpServer.Send(mail));
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending verification code.\n" + ex);
            }
        }

        /// <summary>
        /// Sends verification code to the specified mobile number using Faculti credentials.
        /// </summary>
        public static async Task SendCodeToMobileAsync(string mobileNumber, int code)
        {
            try
            {
                MailMessage mail = new();
                SmtpClient SmtpServer = new("smtp.gmail.com");
                var mobileEmail = SMS.GetCarrierEmail(mobileNumber);

                if (!string.IsNullOrEmpty(mobileEmail))
                {
                    mail.From = new MailAddress("info.faculti@gmail.com");
                    mail.To.Add(mobileEmail);
                    mail.Subject = "Faculti Confirmation Code";
                    mail.Body = "Your Faculti account confirmation code is: " + code + ".";

                    SmtpServer.Port = 587;
                    SmtpServer.Credentials = new System.Net.NetworkCredential("info.faculti", "faculti1234");
                    SmtpServer.EnableSsl = true;

                    await Task.Run(() => SmtpServer.Send(mail));
                }
                else
                {
                    MessageBox.Show("Mobile number carrier is not yet supported.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error sending verification code.\n" + ex);
            }

        }
    }
}