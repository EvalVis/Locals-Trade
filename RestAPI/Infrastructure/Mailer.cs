using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using RestAPI.Models;
using RestAPI.Models.Repositories;

namespace RestAPI.Infrastructure
{
    public class Mailer
    {

        private IServiceRepository repository;
        private IConfiguration config;
        private SmtpClient protocol;

        public Mailer(IServiceRepository repo, IConfiguration configuration)
        {
            repository = repo;
            config = configuration;
            createSmtp();
        }

        public Mailer(IConfiguration configuration)
        {
            config = configuration;
            createSmtp();
        }

        private void createSmtp()
        {
            protocol = new SmtpClient
            {
                Host = "smtp.gmail.com",
                EnableSsl = true,
                UseDefaultCredentials = true,
                Credentials = new NetworkCredential("localstradebox@gmail.com", config["MailPassword"]),
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Port = 587
            };
        }

        public void SendMail(Feedback feedback)
        {
            MailMessage message = new MailMessage();
            message.Subject = "LocalsTrade: Business feedback";
            message.Body = $"Hello. You have received a new business feedback: \"{feedback.Text}\", from {feedback.SenderName}";
            message.IsBodyHtml = false;

            message.From = new MailAddress("localstradebox@gmail.com", "Locals Trade box");
            string toEmail;
            try
            {
                toEmail = repository.Business.Include(b => b.User).First(b => b.BusinessID == feedback.BusinessID).User.Email;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Could not send email: Business somehow does not have an owner or " +
                                $"{nameof(User.Email)} field is null. Business ID: {feedback.BusinessID}. Detailed exception: {e}");
                return;
            }

            message.To.Add(new MailAddress(toEmail, toEmail));

            Task.Run(() =>
            {
                try
                {
                    protocol.Send(message);
                }
                catch (SmtpFailedRecipientException e)
                {
                    Debug.WriteLine($"Failed to send an email (name of the sender: {feedback.SenderName}, message \"{feedback.Text}\") to a recipient {toEmail}. " +
                                    $"Exception code: {e.StatusCode}. Detailed exception info: {e}");
                }
                catch (SmtpException e)
                {
                    Debug.WriteLine($"Failed to send email with smtp. The name of the sender: " +
                                    $" {feedback.SenderName}. The message: \"{feedback.Text}\" was being sent to {toEmail}. " +
                                    $"Exception code: {e.StatusCode}. " +
                                    $"Detailed exception info: {e}");
                }
            });
        }

        public void SendMail(string letter, string toEmail)
        {
            MailMessage message = new MailMessage();
            message.Subject = "LocalsTrade: Potential courier";
            message.Body = $"Hello. We have to inform you, that you have a potential delivery man. Here is what he/she writes to you: \"{letter}\" Have a good day.";
            message.IsBodyHtml = false;

            message.From = new MailAddress("localstradebox@gmail.com", "Locals Trade box");

            message.To.Add(new MailAddress(toEmail, toEmail));
            Task.Run(() =>
            {
                try
                {
                    protocol.Send(message);
                }
                catch (SmtpFailedRecipientException e)
                {
                    Debug.WriteLine("Failed to send an email: " + e.StackTrace);
                }
                catch (SmtpException e)
                {
                    Debug.WriteLine("Failed to send email with smtp: " + e.StackTrace);
                }
            });
        }

    }
}
