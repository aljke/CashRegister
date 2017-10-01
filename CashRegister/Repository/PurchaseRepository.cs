using CashRegister.AppDbContext;
using CashRegister.Entities;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

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
		Task<IEnumerable<CheckData>> GetAllCheckDataAsync();
	}

	public class PurchaseRepository : IPurchaseRepository
	{
		private readonly ApplicationDbContext context;
		private readonly ILogger<PurchaseRepository> _logger;

		public PurchaseRepository(ApplicationDbContext context, ILogger<PurchaseRepository> logger)
		{
			this.context = context;
			_logger = logger;
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
				.Include(x => x.CheckProducts)
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

		public async Task<IEnumerable<CheckData>> GetAllCheckDataAsync()
		{
			var result = await context.Checks
				.Include(x => x.CheckProducts)
				.ToListAsync();
			return result;
		}
	}
}
