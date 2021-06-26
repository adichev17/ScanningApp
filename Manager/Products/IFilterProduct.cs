using ScanningProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Manager.Products
{
    public interface IFilterProduct
    {
      public Task<List<Product>> Filter(ProductViewCategoryOrBrand model);
    }
}
