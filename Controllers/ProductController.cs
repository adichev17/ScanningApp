using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Manager.Products;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly IProductManager _productManager;

        public ProductController(IProductManager productManager)
        {
            _productManager = productManager;
        }

        [HttpPost("/AddProduct")]
        public async Task<IActionResult> AddProduct(Product model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var product = await _productManager.AddProduct(model);
                if (product != null)
                    return Json(product);

                return UnprocessableEntity();
            }
            return Unauthorized();
        }

        [HttpPost("/UpdatePriceProduct")]
        public async Task<IActionResult> UpdatePriceProduct(ProductPriceChangeHistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var UPDproduct = await _productManager.UpdatePriceProduct(model);
                if (UPDproduct != null)
                    return Json(UPDproduct);

                return UnprocessableEntity();
            }
            return BadRequest();
        }

        [HttpPost("/GetProduct")]
        public async Task<IActionResult> GetProduct(ProductViewModel model)
        {
            var ProductWithSelection = await _productManager.GetProduct(model);
            if (ProductWithSelection != null)
                return Json(ProductWithSelection);

            return UnprocessableEntity();
        }
    }
}