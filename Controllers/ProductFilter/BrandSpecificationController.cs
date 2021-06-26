using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ScanningProductsApp.Manager.Products;
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers.ProductFilter
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandSpecificationController : Controller
    {
        private readonly IFilterProduct _filterProduct;

        public BrandSpecificationController(IFilterProduct filterProduct)
        {
            _filterProduct = filterProduct;
        }

        [HttpPost("/GetProductsByBrand")]
        public async Task<IActionResult> GetProductsByBrand(ProductViewCategoryOrBrand model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var products = await _filterProduct.Filter(model);
                if (products != null)
                    return Json(products);

                return UnprocessableEntity();
            }
            return Unauthorized();
        }
    }
}