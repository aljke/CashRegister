using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.ViewModels
{
    public class GeneralDataViewModel
    {
		public int PurchasesCount { get; set; }
		public int TodayPurchases { get; set; }
		public decimal TotalPreceeds { get; set; }
		public decimal TodayPreceeds { get; set; }
    }
}
