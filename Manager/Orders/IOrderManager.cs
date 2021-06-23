using Microsoft.AspNetCore.Mvc;
using ScanningProductsApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ScanningProductsApp.Manager.Orders
{
    public interface IOrderManager
    {
        public Task<HistoryOrders> SubmitPurchase(OrderViewModel model, string UserId);
        public Task<object> GetPurchaseHistory(string UserId);
    }
}
