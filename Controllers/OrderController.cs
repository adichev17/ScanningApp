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

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public OrderController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }
        [HttpPost("/SubmitPurchase/{UserId}")]
        public async Task<IActionResult> SubmitPurchase(OrderViewModel model, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {

                HistoryOrders HistoryOrders = new HistoryOrders { DateTime = DateTime.Now, User = user, TotalСost = model.TotalСost };

                await _context.HistoryOrders.AddAsync(HistoryOrders);
                await _context.SaveChangesAsync();

                var OrderFromHistoryTable = await _context.HistoryOrders.OrderBy(user => user.Id).LastOrDefaultAsync(user => user.UserId == UserId);
                foreach (var ProductBody in model.UPCEANProducts)
                {
                    var product = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == ProductBody.UPCEAN);
                    if (product != null)
                    {
                        for (int i = 0; i < ProductBody.Count; ++i)
                        {
                            Order order = new Order { Product = product, HistoryOrders = OrderFromHistoryTable };
                            await _context.OrdersTable.AddAsync(order);
                            await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }
                return Ok();
            }
            return StatusCode(501);
        }

        [HttpGet("/GetPurchaseHistory/{UserId}")]
        public async Task<IActionResult> GetPurchaseHistory(string UserId) {
            List<object> HistoryOrders = new List<object>();

            var IDUserOrders = await _context.HistoryOrders.Where(userId => userId.UserId == UserId)
                .Select(user => user.Id)
                .ToListAsync();

            if (IDUserOrders.Count == 0)
                return NotFound();

            foreach (var IDitem in IDUserOrders)
            {
                var FullOrder = new HistoryOrderViewModel();
                var order = await _context.HistoryOrders.FirstOrDefaultAsync(ord => ord.Id == IDitem);

                var IDproducts = await _context.OrdersTable.Where(order => order.OrderId == IDitem)
                    .Select(product => product.ProductId)
                    .ToListAsync();

                List<Product> product = new List<Product>();
                foreach (var IDproduct in IDproducts)
                {
                    product.Add(await _context.ProductTable.FirstOrDefaultAsync(product => product.Id == IDproduct));
                }

                FullOrder.DateTime = order.DateTime;
                FullOrder.TotalСost = order.TotalСost;
                FullOrder.ProductsIncludeOrder = product;

                HistoryOrders.Add(FullOrder);
            }
            return Json(HistoryOrders);
        }
    }
}