using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ScanningProductsApp.Domain;
using ScanningProductsApp.Manager.Orders;
using ScanningProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Manager.Products
{
    public class ProductManager : IProductManager
    {
        private readonly AppDbContext _context;

        public ProductManager(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Product> AddProduct(Product model)
        {

            var FindDuplicate = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEAN);
            if (FindDuplicate != null)
                return null;

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
                }
                else
                {
                    return null;
                }
                return newProduct;
            }
            return null;
        }

        public async Task<ResponseRequestProduct> GetProduct(ProductViewModel model)
        {
            var product = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEAN);
            if (product == null)
                return null;

            var RelatedProductsSorted = await _context.SelectionForProducts
                .Where(position => position.AdjacentProduct == product || position.Product == product)
                .OrderByDescending(position => position.Count)
                .Select(position => position.AdjacentProduct == product ? position.Product : position.AdjacentProduct)
                .Take(5)
                .ToListAsync();

            if (RelatedProductsSorted == null)
                return null;


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

            return answer;
        }

        public async Task<Product> UpdatePriceProduct(ProductPriceChangeHistoryViewModel model)
        {
            var product = await _context.ProductTable.FirstOrDefaultAsync(product => product.UPCEAN == model.UPCEAN);
            if (product == null)
                return null;
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
            return product;
        }
    }
}
