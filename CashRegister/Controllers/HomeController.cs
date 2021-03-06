﻿using System;
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
using Microsoft.Extensions.Logging;

namespace CashRegister.Controllers
{
    public class HomeController : Controller
    {
		private readonly IPurchaseRepository purchaseRepo;
		private readonly ILogger<HomeController> _logger;

		public HomeController(IPurchaseRepository purchaseRepository, ILogger<HomeController> logger)
		{
			purchaseRepo = purchaseRepository;
			_logger = logger;
		}

        public IActionResult Index()
        {
			var products = purchaseRepo.GetAllProductsAsync();
			ViewBag.ProductsSelect = new SelectList(products, "Id", "Caption");

			return View();
        }

		[HttpGet("api/GetGeneralData")]
		public async Task<IActionResult> GetGeneralData()
		{
			var allPurchases = await purchaseRepo.GetAllCheckDataAsync();
			var todayPurchases = allPurchases.Where(x => x.UtcTime.Day == DateTime.UtcNow.Day);

			var viewModel = new GeneralDataViewModel();
			viewModel.PurchasesCount = allPurchases.Count();
			viewModel.TotalPreceeds = allPurchases.Sum(x => x.CheckProducts.Sum(y => y.Price * y.Amount));
			viewModel.TodayPurchases = todayPurchases.Count();
			viewModel.TodayPreceeds = todayPurchases.Sum(x => x.CheckProducts.Sum(y => y.Price * y.Amount));

			try
			{
				return Ok(JsonConvert.SerializeObject(viewModel));
			}
			catch (Exception ex)
			{
				_logger.LogCritical(ex.StackTrace); 
				return new BadRequestResult();
			}
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
				_logger.LogCritical(ex.StackTrace); // something is bad
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
