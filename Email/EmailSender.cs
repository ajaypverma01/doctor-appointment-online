using Azure;
using Azure.Communication.Email;
using DoctorPatient.Models;
using DoctorPatient.ViewModels;
using Microsoft.AspNet.Identity;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics.Metrics;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Web.SessionState;
using static DoctorPatient.Models.Enums;


namespace DoctorPatient.Email
{
    public static class EmailSender
    {
        public static void SendEmail(EmailBody emailBody )
        {
            var connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["EmailSenderConnection"].ConnectionString;
          

            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12 | System.Net.SecurityProtocolType.Tls | System.Net.SecurityProtocolType.Tls11;

            var emailClient = new EmailClient(connectionString);


            //Fetching Email Body Text from EmailTemplate File.  
            string FilePath = string.Empty;
            var appointmentType = string.Empty;
            switch (emailBody.EmailType)
            {
                case Enums.EmailType.AppointmentConfirmation:
                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\AppointmentConfirmationEmail.html";
                    appointmentType = "confirmed";
                    break;
                case Enums.EmailType.AppointmentReschedule:
                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\AppointmentRescheduleEmail.html";
                    appointmentType = "rescheduled";
                    break;
                case Enums.EmailType.AppointmentCancellation:
                    FilePath = AppDomain.CurrentDomain.BaseDirectory + "\\Email\\AppointmentCancellationEmail.html";
                    appointmentType = "cancelled";
                    break;
                default:
                    break;
            }     
           

            string MailText = File.ReadAllText(FilePath);
          
            MailText = MailText.Replace("[PatientName]", emailBody.PatientName);
            MailText = MailText.Replace("[DoctorName]", emailBody.DoctorName);
            MailText = MailText.Replace("[AppointmentDate]", emailBody.AppointmentDate);
            MailText = MailText.Replace("[AppointmentTime]", emailBody.AppointmentTime);
            MailText = MailText.Replace("[ClinicName]", emailBody.ClinicName);
            MailText = MailText.Replace("[ClinicPhoneNumber]", emailBody.ClinicPhoneNumber);
            MailText = MailText.Replace("[ClinicAddress]", emailBody.ClinicAddress);
            MailText = MailText.Replace("[AppointmentID]", emailBody.AppointmentID);

            

            var emailContent = new EmailContent($"Appointment {appointmentType} with {emailBody.DoctorName} for {emailBody.AppointmentDate} at {emailBody.AppointmentTime}")
            {
                
                Html = MailText

            };

            List<EmailAddress> toRecipients = new List<EmailAddress> { new EmailAddress(emailBody.EmailTo)};
            //List<EmailAddress> ccRecipients = new List<EmailAddress> { new EmailAddress("probable.ajay@gmail.com") };
            //List<EmailAddress> bccRecipients = new List<EmailAddress> { new EmailAddress("ajay.pverma01@gmail.com") };
   

            //var emailRecipients = new EmailRecipients(toRecipients, ccRecipients, bccRecipients);
            var emailRecipients = new EmailRecipients(toRecipients);
            var emailFrom = System.Configuration.ConfigurationManager.AppSettings["EmailFrom"].ToString(); ;
            var emailMessage = new EmailMessage(emailFrom, emailRecipients, emailContent);


            try
            {
                var emailSendOperation = emailClient.Send(WaitUntil.Completed, emailMessage);
                Console.WriteLine($"Email Sent. Status = {emailSendOperation.Value.Status}");

                var operationId = emailSendOperation.Id;
                Console.WriteLine($"Email operation id = {operationId}");

            
            }
            catch (RequestFailedException ex)
            {
                Console.WriteLine($"Email send operation failed with error code: {ex.ErrorCode}, message: {ex.Message}");
            }
        }
    }
}
