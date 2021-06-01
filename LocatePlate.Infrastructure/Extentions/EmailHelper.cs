using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace LocatePlate.Infrastructure.Extentions
{
    public static class EmailHelper
    {
        public static void Email(string email, string subject, string htmlString, string attechment = "")
        {
            try
            {
                MailMessage message = new MailMessage();
                SmtpClient smtp = new SmtpClient();
                message.From = new MailAddress("support@locateplate.com");
                message.To.Add(new MailAddress($"{email}"));
                message.Subject = subject;
                message.IsBodyHtml = true; //to make message body as html  
                message.Body = htmlString;
                if (!string.IsNullOrEmpty(attechment))
                {
                    if (File.Exists(attechment))
                    {
                        Attachment attachment = new Attachment(attechment);
                        message.Attachments.Add(attachment);
                    }
                }

                //smtp.Port = 465;
                smtp.Port = 587;
                smtp.Host = "smtpout.secureserver.net";
                smtp.EnableSsl = true;
                //smtp.EnableSsl = false;
                smtp.UseDefaultCredentials = false;
                smtp.Credentials = new NetworkCredential("support@locateplate.com", "Welcome2020");
                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
                smtp.Send(message);
            }
            catch (Exception ex) { }
        }


        public static string PrepareWelcomeEmail(string userName, string confirmationLink, string SenderName)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"wwwroot\email-templates\Registration-and-Welcome-Email-Template.txt");

                text = text.Replace("#UserName#", userName);
                text = text.Replace("#confirmation#", confirmationLink);
                text = text.Replace("#Sender_Address#", SenderName);
                return text;
            }
            catch (Exception)
            {
                return "";
            }
        }
        public static string UserMailOrderConfirmation(string restaurantName, string orderId, string dateTime,string time, int noOfGuest)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"wwwroot\email-templates\User-Order-receive-confirmation-mail.html");

                text = text.Replace("#RestaurantName#", restaurantName);
                text = text.Replace("#OrderId#", orderId);
                text = text.Replace("#DateTime#", dateTime);
                text = text.Replace("#Noofguest#", noOfGuest.ToString());
                text = text.Replace("#Time#", time);
                return text;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string MailOrderConfirmation(string restaurantName, string orderId, string dateTime, string time, int noOfGuest)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"wwwroot\email-templates\RestaurantOrderReceivedMail.html");

                text = text.Replace("#RestaurantName#", restaurantName);
                text = text.Replace("#OrderId#", orderId);
                text = text.Replace("#Date#", dateTime);
                text = text.Replace("#Noofguest#", noOfGuest.ToString());
                text = text.Replace("#Time#", time);
                return text;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string OrderRejectMail(string orderSatus,string orderId, string dateTime, string time, int noOfGuest)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"wwwroot\email-templates\Order_Reject_Mail.html");

                text = text.Replace("#OrderStatus#", orderSatus);
                text = text.Replace("#OrderNo#", orderId);
                text = text.Replace("#Date#", dateTime);
                text = text.Replace("#Time#", time);
                text = text.Replace("#Noofguest#", noOfGuest.ToString());
                return text;
            }
            catch (Exception)
            {
                return "";
            }
        }

        public static string AdminContactUsEmail(string Name, string Email, string Mobilenumber, string Message)
        {
            try
            {
                string text = System.IO.File.ReadAllText(@"wwwroot\email-templates\ContactUs_EmailTemplate.html");

                text = text.Replace("#Name#", Name);
                text = text.Replace("#Email#", Email);
                text = text.Replace("#Mobilenumber#", Mobilenumber);
                text = text.Replace("#Message#", Message);
                //text = text.Replace("#Noofguest#", noOfGuest.ToString());
                return text;
            }
            catch (Exception)
            {
                return "";
            }
        }


    }

}
