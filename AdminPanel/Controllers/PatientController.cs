using AdminPanel.Models;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mail;

using System;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MimeKit;

namespace AdminPanel.Controllers
{
    public class PatientController : Controller
    {
        public IActionResult form()
        
        {
            SendEmailAsync();
            return View();
        }

        [HttpPost]
        public IActionResult form( Patientmaster data)
        {
            return View();
        }


        public IActionResult List()
        {
            return View();
        }






        public static async Task SendEmailAsync()
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("Your Name", "rutikthorat2904@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("Recipient Name", "rutikthorat3023@gmail.com"));
            emailMessage.Subject = "Test Email from .NET Core";
            emailMessage.Body = new TextPart("plain") { Text = "Hello, this is a test email sent from a .NET Core application using Gmail SMTP!" };

            // Explicitly use the MailKit SmtpClient
            using (var client = new MailKit.Net.Smtp.SmtpClient()) // Fully qualified name
            {
                try
                {
                    await client.ConnectAsync("smtp.gmail.com", 587, false);
                    await client.AuthenticateAsync("rutikthorat2904@gmail.com", "hmml lrdk xubh zawm");
                    await client.SendAsync(emailMessage);
                    await client.DisconnectAsync(true);
                    Console.WriteLine("Email sent successfully!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error sending email: {ex.Message}");
                }
            
             }
        }

        
    }















}

