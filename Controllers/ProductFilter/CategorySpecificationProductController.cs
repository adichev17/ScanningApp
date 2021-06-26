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
    public class CategorySpecificationProductController : Controller
    {
        private readonly IFilterProduct _filterProduct;

        public CategorySpecificationProductController(IFilterProduct filterProduct)
        {
            _filterProduct = filterProduct;
        }

        //not connected

        [HttpPost("/GetProductsByCategory")]
        public async Task<IActionResult> GetProductsByCategory(ProductViewCategoryOrBrand model)
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