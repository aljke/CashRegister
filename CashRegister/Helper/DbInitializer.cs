using CashRegister.AppDbContext;
using CashRegister.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CashRegister.Helper
{
	public class DbInitializer
    {
		private ApplicationDbContext context { get; set; }

		public DbInitializer(ApplicationDbContext context)
		{
			this.context = context;
		}

		public async void Initialize()
		{
			await InitializeDefaultProductsAsync();

			await context.SaveChangesAsync();
		}

		private async Task InitializeDefaultProductsAsync()
		{
			if (!context.Products.Any())
			{
				var defaultProducts = new List<Product>
				{
					new Product { Caption = "Milk"},
					new Product { Caption = "Bread" },
					new Product { Caption = "Borsh" },
					new Product { Caption = "Salad Caesar big"},
					new Product { Caption = "Salad Caesar small"},
					new Product { Caption = "Vareniki with cherries"},
					new Product {Caption = "Mashed potatoes"}
				};

				await context.Products.AddRangeAsync(defaultProducts);
			}
		}
    }
}
