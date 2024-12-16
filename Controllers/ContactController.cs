using Allurerealstate.Models;
using Microsoft.AspNetCore.Mvc;
using RestSharp;
using System.Diagnostics;
using System.Net.Mail;
using System.Net;

namespace Allurerealstate.Controllers
{
    public class ContactController : Controller
    {
        public IActionResult Index()
        {
            return View(new FormModel());
        }

        [HttpPost]
        public IActionResult SendMessage(FormModel model)
        {
            if (!ModelState.IsValid)
            {
                TempData["MessageType"] = "danger";
                TempData["Message"] = "Invalid form data!";
                return RedirectToAction("Index");
            }

            var captchaToken = Request.Form["g-recaptcha-response"];
            if (!VerifyCaptcha(captchaToken))
            {
                TempData["Message"] = "Captcha verification failed. Please try again.";
                return RedirectToAction("Index");
            }

            var templatePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "mailtemp", "mailtemp.html");
            var emailTemplate = System.IO.File.ReadAllText(templatePath);

            emailTemplate = emailTemplate.Replace("{{FirstName}}", model.FirstName)
                                         .Replace("{{LastName}}", model.LastName)
                                         .Replace("{{Email}}", model.Email)
                                         .Replace("{{Phone}}", model.Phone)
                                         .Replace("{{Message}}", model.Message);

            var client = new SmtpClient()
            {
                Credentials = new NetworkCredential("postmaster@", ""),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("postmaster@", "Contact Form"),
                Subject = "New Message",
                Body = emailTemplate,
                IsBodyHtml = true
            };

            mailMessage.To.Add(new MailAddress(""));

            client.Send(mailMessage);

            TempData["MessageType"] = "success";
            TempData["Message"] = "Message sent successfully!";
            return RedirectToAction("Index");

        }

        private bool VerifyCaptcha(string captchaToken)
        {
            var client = new RestClient("https://www.google.com/recaptcha/api/siteverify");
            var request = new RestRequest
            {
                Method = Method.Post
            };
            request.AddParameter("secret", "");
            request.AddParameter("response", captchaToken);

            var response = client.Execute<CaptchaResponse>(request);

            return response.Data != null && response.Data.Success && response.Data.Score > 0.6;
        }
    }
}
