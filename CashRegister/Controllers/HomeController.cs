using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CashRegister.Repository;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json.Linq;
using CashRegister.Entities;
using CashRegister.ViewModels;
using Newtonsoft.Json;

namespace CashRegister.Controllers
{
    public class HomeController : Controller
    {
		private readonly IPurchaseRepository purchaseRepo;

		public HomeController(IPurchaseRepository purchaseRepository)
		{
			purchaseRepo = purchaseRepository;
		}

        public IActionResult Index()
        {
			var products = purchaseRepo.GetAllProductsAsync();
			ViewBag.ProductsSelect = new SelectList(products, "Id", "Caption");
            return View();
        }

		[HttpPost]
		public async Task<IActionResult> PerformPurchase([FromBody]JToken json)
		{
			var checkProducts = new List<CheckProduct>();

			//insert new check into DB
			var check = new CheckData();
			check.UtcTime = DateTime.UtcNow;
			await purchaseRepo.AddCheckData(check);

			//parse products info from JSON
			foreach(var item in json.Children().Children())
			{
				var productId = item.Value<int>("productId");
				var price = item.Value<decimal>("price");
				var amount = item.Value<int>("amount");

				checkProducts.Add(new CheckProduct
				{
					ProductId = productId,
					Price = price,
					Amount = amount,
					CheckId = check.Id
				});
			}

			try
			{
				//insert info about bought products into DB
				await purchaseRepo.AddCheckProductsAsync(checkProducts);

				// return check to the client
				return Ok(JsonConvert.SerializeObject(await ReturnCheck(check.Id)));
			}
			catch(Exception ex)
			{
				return new BadRequestResult();
			}
		}

        public IActionResult Error()
        {
            return View();
        }

		private async Task<CheckViewModel> ReturnCheck(int checkId)
		{
			var check = await purchaseRepo.GetCheckDataAsync(checkId);
			var checkProducts = check.CheckProducts
				.Select(x => new ProductDataViewModel
				{
					Amount = x.Amount,
					Caption = purchaseRepo.GetProduct(x.ProductId).Caption,
					Price = x.Price
				})
				.ToList();

			var totalPrice = checkProducts.Sum(x => x.Price * x.Amount);

			return new CheckViewModel
			{
				DateTime = check.UtcTime,
				Products = checkProducts,
				TotalPrice = totalPrice
			};
		}
    }
}
