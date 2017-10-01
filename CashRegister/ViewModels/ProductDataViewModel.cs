using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.ViewModels
{
    public class ProductDataViewModel
    {
		public string Caption { get; set; }
		public decimal Price { get; set; }
		public int Amount { get; set; }
	}
}
