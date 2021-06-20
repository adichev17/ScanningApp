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
using ScanningProductsApp.Models;

namespace ScanningProductsApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly AppDbContext _context;

        public ProductController(UserManager<User> userManager, SignInManager<User> signInManager, AppDbContext context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _context = context;
        }

        [HttpPost("/AddProduct")]
        public async Task<IActionResult> AddProduct(Product model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var FindDuplicate = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEAN);
                if (FindDuplicate != null)
                    return StatusCode(403);
                var Brand = await _context.BrandCategory.FirstOrDefaultAsync(brand => brand.Id == model.BrandID);
                var Category = await _context.CategoryTable.FirstOrDefaultAsync(category => category.Id == model.CategoryID);
                var UnitOfAccount = await _context.UnitOfAccount.FirstOrDefaultAsync(UoA => UoA.Id == model.UnitOfAccountID);
                if (Brand != null && Category != null && UnitOfAccount != null)
                {
                    Product newProduct = new Product
                    {
                        UPCEAN = model.UPCEAN,
                        Name = model.Name,
                        Description = model.Description,
                        Category = Category,
                        Brand = Brand,
                        UnitOfAccount = UnitOfAccount,
                        Price = model.Price,
                        IsSale = model.IsSale,
                        Image = model.Image
                    };
                    await _context.ProductTable.AddAsync(newProduct);
                    await _context.SaveChangesAsync();

                    var product = await _context.ProductTable.FirstOrDefaultAsync(pt => pt.UPCEAN == newProduct.UPCEAN);
                    if (product != null)
                    {
                        //Save price for history 
                        PriceChangeHistory PriceChangeHistory = new PriceChangeHistory
                        {
                            Product = product,
                            DateTime = DateTime.Now,
                            Price = product.Price,
                            IsSale = product.IsSale
                        };
                        await _context.PriceChangeHistory.AddAsync(PriceChangeHistory);
                        await _context.SaveChangesAsync();
                        //
                    }
                    else
                    {
                        return StatusCode(500);
                    }
                    return Ok();
                }
                return BadRequest();
            }
            return Unauthorized();
        }

        [HttpPost("/UpdatePriceProduct")]
        public async Task<IActionResult> UpdatePriceProduct(ProductPriceChangeHistoryViewModel model)
        {
            if (ModelState.IsValid)
            {
                var product = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEAN);
                if (product == null)
                    return UnprocessableEntity();
                product.Price = model.Price;
                product.IsSale = model.IsSale;

                PriceChangeHistory PriceChangeHistory = new PriceChangeHistory
                {
                    Product = product,
                    DateTime = DateTime.Now,
                    Price = model.Price,
                    IsSale = model.IsSale
                };
                await _context.PriceChangeHistory.AddAsync(PriceChangeHistory);
                await _context.SaveChangesAsync();
                return Json(product);
            }
            return BadRequest();
        }

        [HttpPost("/GetProduct")]
        public async Task<IActionResult> GetProduct(ProductViewModel model)
        {

            var product = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEAN);
            if (product == null)
                return UnprocessableEntity();

            var RelatedProductsSorted = await _context.SelectionForProducts
                .Where(position => position.AdjacentProduct == product || position.Product == product)
                .OrderByDescending(position => position.Count)
                .Select(position => position.AdjacentProduct == product ? position.Product : position.AdjacentProduct)
                .Take(5)
                .ToListAsync();

            if (RelatedProductsSorted == null)
                return UnprocessableEntity();


            if (RelatedProductsSorted.Count < 6)
            {
                await foreach (var productFromDB in _context.ProductTable)
                {
                    if (!RelatedProductsSorted.Contains(productFromDB) && productFromDB != product)
                    {
                        RelatedProductsSorted.Add(productFromDB);
                    }
                    if (RelatedProductsSorted.Count == 6)
                        break;
                }
            }

            ResponseRequestProduct answer = new ResponseRequestProduct { Product = product, RelatedProducts = RelatedProductsSorted };
            return Json(answer);
        }

        [HttpPost("/GetProductsByCategory")]
        public async Task<IActionResult> GetProductsByCategory(ProductViewCategoryOrBrand model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var products = await _context.ProductTable.Where(vl => vl.CategoryID == model.IDNameDivisions).ToListAsync();
                if (products != null)
                    return Json(products);
                return NotFound();
            }
            return Unauthorized();
        }

        [HttpPost("/GetProductsByBrand")]
        public async Task<IActionResult> GetProductsByBrand(ProductViewCategoryOrBrand model)
        {
            if (User.Identity.IsAuthenticated)
            {
                var products = await _context.ProductTable.Where(vl => vl.BrandID == model.IDNameDivisions).ToListAsync();
                if (products != null)
                    return Json(products);
                return NotFound();
            }
            return Unauthorized();
        }
    }
}