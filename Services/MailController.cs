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
        private readonly IMessageSender _messageSender;

        public MailController(IMessageSender messageSender)
        {
            _messageSender = messageSender;
        }


        [HttpPost("/SendEmailForFeedback")]
        public async Task<IActionResult> SendEmailForFeedback(FeedVackViewModel model)
        {
            var message = await _messageSender.Send(model);
            if (message != null)
                return Ok();

            return UnprocessableEntity();
        }
    }
}