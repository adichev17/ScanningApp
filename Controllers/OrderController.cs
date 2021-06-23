using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Models;
using System.Collections;
using Microsoft.AspNetCore.Authorization;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using ScanningProductsApp.Manager.Orders;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : Controller
    {

        private readonly IOrderManager _orderManager;
        public OrderController(UserManager<User> userManager, AppDbContext context, IOrderManager orderManager)
        {
            _orderManager= orderManager;
        }

        [HttpPost("/SubmitPurchase/{UserId}")]
        public async Task<IActionResult> SubmitPurchase(OrderViewModel model, string UserId)
        {
            var order = await _orderManager.SubmitPurchase(model,UserId);
            if (order != null)
            {
                return Json(order);
            }
            return Unauthorized();
        }

        [HttpGet("/GetPurchaseHistory/{UserId}")]
        public async Task<IActionResult> GetPurchaseHistory(string UserId) {

            var HistoryOrders = await _orderManager.GetPurchaseHistory(UserId);

            if (HistoryOrders != null)
                return Json(HistoryOrders);
            
            return UnprocessableEntity();
        }

    }
}