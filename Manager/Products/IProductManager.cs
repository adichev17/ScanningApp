using Microsoft.AspNetCore.Mvc;
using ScanningProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Manager.Products
{
    public interface IProductManager
    {
        public Task<Product> AddProduct(Product model);
        public Task<Product> UpdatePriceProduct(ProductPriceChangeHistoryViewModel model);
        public Task<ResponseRequestProduct> GetProduct(ProductViewModel model);
    }
}
