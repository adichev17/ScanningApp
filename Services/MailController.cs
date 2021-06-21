using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Services
{
    [Route("api/[controller]")]
    [ApiController]
    public class MailController : Controller
    {
        [HttpPost("/SendEmailForFeedback")]
        public async Task<IActionResult> SendEmailForFeedback(FeedVackViewModel model)
        {
            SmtpClient Smtp = new SmtpClient("smtp.yandex.com", 587);
            Smtp.Credentials = new NetworkCredential("ScanningApp2021@yandex.ru", "ScanningAppADEKDE");
            MailMessage Message = new MailMessage();
            Smtp.EnableSsl = true;
            Message.From = new MailAddress("ScanningApp2021@yandex.ru");
            Message.To.Add(new MailAddress("ScanningApp2021@yandex.ru"));
            Message.Subject = "От " + model.Email + "   " + "Тема: " + model.Theme;
            Message.Body = model.Message;

            try
            {
                await Smtp.SendMailAsync(Message);
            }
            catch (SmtpException ex)
            {
                return Json(ex.Message);
            }
            return Ok();
        }
    }
}