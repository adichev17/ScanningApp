using ScanningProductsApp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace ScanningProductsApp.Domain
{
    public class AppDbContext : IdentityDbContext<User>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> TableUsers { get; set; }

        [Display(Name = "ProductTable")]
        public DbSet<Product> ProductTable { get; set; }
        [Display(Name = "CategoryTable")]
        public DbSet<Category> CategoryTable { get; set; }
        [Display(Name = "BrandCategory")]
        public DbSet<Brand> BrandCategory { get; set; }
        [Display(Name = "UnitOfAccount")]
        public DbSet<UnitOfAccount> UnitOfAccount { get; set; }
        [Display(Name = "HistoryOrders")]
        public DbSet<HistoryOrders> HistoryOrders { get; set; }
        [Display(Name = "OrdersTable")]
        public DbSet<Order> OrdersTable { get; set; }
        [Display(Name = "PriceChangeHistory")]
        public DbSet<PriceChangeHistory> PriceChangeHistory { get; set; }
        [Display(Name = "SelectionForProducts")]
        public DbSet<SelectionForProducts> SelectionForProducts { get; set; }
    }
}
