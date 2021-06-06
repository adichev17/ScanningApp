using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpPost("/GetProduct")]
        public async Task<IActionResult> GetProduct(ProductViewModel model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var product = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEAN);
                return Json(product);
            }
            return Unauthorized();
        }
    }
}