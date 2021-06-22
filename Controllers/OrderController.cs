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

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class OrderController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly AppDbContext _context;

        public OrderController(UserManager<User> userManager, AppDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        private async Task<IActionResult> AddingDependencieForPurchases(OrderViewModel model)
        {
            for (int i = 0; i < model.UPCEANProducts.Count - 1; ++i)
            {
                var productFromDB = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEANProducts[i].UPCEAN);
                if (productFromDB == null)
                    return UnprocessableEntity();
                for (int j = i + 1; j < model.UPCEANProducts.Count; j++)
                {
                    var RelatedProductsFromDB = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEANProducts[j].UPCEAN);
                    if (RelatedProductsFromDB == null)
                        return UnprocessableEntity();

                    var RelatedProducts = await _context.SelectionForProducts
                    .FirstOrDefaultAsync(position => (position.Product == productFromDB && position.AdjacentProduct == RelatedProductsFromDB)
                    || (position.Product == RelatedProductsFromDB && position.AdjacentProduct == productFromDB));

                    if (RelatedProducts != null)
                    {
                        RelatedProducts.Count++;
                        //await _context.SaveChangesAsync();
                    }
                    else
                    {
                        SelectionForProducts SelectionForProducts = new SelectionForProducts { Product = productFromDB, AdjacentProduct = RelatedProductsFromDB, Count = 1 };
                        await _context.SelectionForProducts.AddAsync(SelectionForProducts);
                        //await _context.SaveChangesAsync();
                    }
                }
            };
            await _context.SaveChangesAsync();
            return Ok();
        }

        [HttpPost("/SubmitPurchase/{UserId}")]
        public async Task<IActionResult> SubmitPurchase(OrderViewModel model, string UserId)
        {
            var user = await _userManager.FindByIdAsync(UserId);
            if (user != null)
            {
                HistoryOrders HistoryOrders = new HistoryOrders { DateTime = DateTime.Now.AddHours(3), User = user, TotalСost = model.TotalСost };

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
                            //await _context.SaveChangesAsync();
                        }
                    }
                    else
                    {
                        return BadRequest();
                    }
                }

                // adding dependencies for purchases(With this often take)
                await AddingDependencieForPurchases(model);

                await _context.SaveChangesAsync();
                return Ok();
            }
            return Unauthorized();
        }

        [HttpGet("/GetPurchaseHistory/{UserId}")]
        public async Task<IActionResult> GetPurchaseHistory(string UserId) {
            List<object> HistoryOrders = new List<object>();

            var IDUserOrders = await _context.HistoryOrders.Where(userId => userId.UserId == UserId)
                .Select(user => user.Id)
                .ToListAsync();

            if (IDUserOrders.Count == 0)
                return UnprocessableEntity();

            foreach (var IDitem in IDUserOrders)
            {
                var FullOrder = new HistoryOrderViewModel(); 
                var order = await _context.HistoryOrders.FirstOrDefaultAsync(ord => ord.Id == IDitem);

                var IDproducts = await _context.OrdersTable.Where(order => order.OrderId == IDitem)
                    .Select(product => product.ProductId)
                    .ToListAsync();

                List<ProductAndQuantityModel> ProductAndQuantityModel = new List<ProductAndQuantityModel>();
                foreach (var IDproduct in IDproducts)
                {
                    var product = await _context.ProductTable.FirstOrDefaultAsync(product => product.Id == IDproduct);
                    var searchProductinList = ProductAndQuantityModel.Find(pr => pr.Product == product);

                    if (searchProductinList != null)
                    {
                        searchProductinList.Count++;
                    } else
                    {
                        ProductAndQuantityModel model = new ProductAndQuantityModel
                        {
                            Product = product,
                            Count = 1
                        };
                        ProductAndQuantityModel.Add(model);
                    }
                }
                FullOrder.DateTime = order.DateTime;
                FullOrder.TotalСost = order.TotalСost;
                FullOrder.ProductsIncludeOrder = ProductAndQuantityModel;

                HistoryOrders.Add(FullOrder);
            }
            return Json(HistoryOrders);
        }
    }
}