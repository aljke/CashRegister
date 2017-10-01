using CashRegister.AppDbContext;
using CashRegister.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.Repository
{
	public interface IPurchaseRepository
    {
		IEnumerable<Product> GetAllProductsAsync();
		Task<CheckData> GetCheckDataAsync(int id);
		Task<Product> GetProductAsync(int id);
		Product GetProduct(int id);
		Task AddCheckProductsAsync(IEnumerable<CheckProduct> products);
		Task AddCheckData(CheckData data);
	}

	public class PurchaseRepository : IPurchaseRepository
	{
		private readonly ApplicationDbContext context;

		public PurchaseRepository(ApplicationDbContext context)
		{
			this.context = context;
		}

		public IEnumerable<Product> GetAllProductsAsync()
		{
			return context.Products.ToList();
		}

		public async Task AddCheckProductsAsync(IEnumerable<CheckProduct> products)
		{
			await context.CheckProducts.AddRangeAsync(products);
			await context.SaveChangesAsync();
		}

		// should be synchronous
		public async Task AddCheckData(CheckData data)
		{
			var check = await context.Checks.AddAsync(data);
			await context.SaveChangesAsync();
		}

		public async Task<CheckData> GetCheckDataAsync(int id)
		{
			var result = await context.Checks
				.FirstOrDefaultAsync(x => x.Id == id);

			return result;
		}

		public async Task<Product> GetProductAsync(int id)
		{
			var result = await context.Products
				.FirstOrDefaultAsync(x => x.Id == id);

			return result;
		}

		public Product GetProduct(int id)
		{
			return context.Products
				.FirstOrDefault(x => x.Id == id);
		}
	}
}
