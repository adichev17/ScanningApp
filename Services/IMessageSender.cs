using Microsoft.AspNetCore.Mvc;
using ScanningProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Services
{
    public interface IMessageSender
    {
        public Task<string?> Send(FeedVackViewModel model);
    }
}
