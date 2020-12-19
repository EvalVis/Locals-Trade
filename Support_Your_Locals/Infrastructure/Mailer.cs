using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Support_Your_Locals.Controllers;
using Support_Your_Locals.Models;
using Support_Your_Locals.Models.Repositories;

namespace Support_Your_Locals.Infrastructure
{
    public class Mailer
    {

        private IServiceRepository repository;
        private IConfiguration config;

        private Lazy<SmtpClient> smtp;

        public Mailer(IApplicationBuilder app, IConfiguration configuration)
        {
            repository = app.ApplicationServices.CreateScope().ServiceProvider.GetRequiredService<IServiceRepository>();
            config = configuration;
            smtp = new Lazy<SmtpClient>(() =>
            {
                return new SmtpClient()
                {
                    Host = "smtp.gmail.com",
                    EnableSsl = true,
                    UseDefaultCredentials = true,
                    Credentials = new NetworkCredential("localstradebox@gmail.com", config["MailPassword"]),
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Port = 587
                };
            });
            BusinessController.FeedbackEvent += SendMail;
            AdminController.ResponseEvent += AnswerQuestion;
        }


        public void SendMail(object sender, FeedbackEventArgs feedbackEvent)
        {
            MailMessage message = new MailMessage();
            message.Subject = "LocalsTrade: Business feedback";
            message.Body = $"Hello. You have received a new business feedback: \"{feedbackEvent.Feedback.Text}\", from {feedbackEvent.Feedback.SenderName}";
            message.IsBodyHtml = false;

            message.From = new MailAddress("localstradebox@gmail.com", "Locals Trade box");
            string toEmail;
            try
            {
                toEmail = repository.Business.Where(b => b.BusinessID == feedbackEvent.Feedback.BusinessID).Include(b => b.User)
                    .First().User.Email;
            }
            catch (Exception e)
            {
                Debug.WriteLine($"Could not send email: Business somehow does not have an owner or " +
                                $"{nameof(User.Email)} field is null. Business ID: {feedbackEvent.Feedback.BusinessID}. Detailed exception: {e}");
                return;
            }

            message.To.Add(new MailAddress(toEmail, toEmail));
            SmtpClient protocol = smtp.Value;
            Task.Run(() =>
            {
                try
                {
                    protocol.Send(message);
                }
                catch (SmtpFailedRecipientException e)
                {
                    Debug.WriteLine($"Failed to send an email (name of the sender: {feedbackEvent.Feedback.SenderName}, message \"{feedbackEvent.Feedback.Text}\") to a recipient {toEmail}. " +
                                    $"Exception code: {e.StatusCode}. Detailed exception info: {e}");
                }
                catch (SmtpException e)
                {
                    Debug.WriteLine($"Failed to send email with smtp. The name of the sender: " +
                                    $" {feedbackEvent.Feedback.SenderName}. The message: \"{feedbackEvent.Feedback.Text}\" was being sent to {toEmail}. " +
                                    $"Exception code: {e.StatusCode}. " +
                                    $"Detailed exception info: {e}");
                }
            });
        }
		
        public void AnswerQuestion(object sender, ResponseEventArgs responseEvent)
        {
            MailMessage message = new MailMessage();
            message.Subject = "LocalsTrade: Response to a question";
            message.Body = responseEvent.Response;
            message.IsBodyHtml = false;
            message.From = new MailAddress("localstradebox@gmail.com", "Locals Trade box");
            message.To.Add(new MailAddress(responseEvent.Email, responseEvent.Email));
            
            SmtpClient protocol = smtp.Value;
            Task.Run(() =>
            {
                try
                {
                    protocol.Send(message);
                }
                catch (SmtpException e)
                {
                    Debug.WriteLine($"Failed to send email with smtp. The message: \"{responseEvent.Response}\" was being sent to {responseEvent.Email}. " +
                                    $"Exception code: {e.StatusCode}. " +
                                    $"Detailed exception info: {e}");
                }
            });
        }

        public void Mute()
        {
            BusinessController.FeedbackEvent -= SendMail;
            AdminController.ResponseEvent -= AnswerQuestion;
        }

    }
}
