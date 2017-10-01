using CashRegister.Entities;
using System;
using System.Collections.Generic;

namespace CashRegister.ViewModels
{
	public class CheckViewModel
    {
		public DateTime DateTime { get; set; }
		public List<ProductDataViewModel> Products { get; set; }
		public decimal TotalPrice { get; set; }
    }
}
