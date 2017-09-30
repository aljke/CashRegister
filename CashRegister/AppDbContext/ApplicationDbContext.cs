using CashRegister.Entities;
using Microsoft.EntityFrameworkCore;

namespace CashRegister.AppDbContext
{
	public class ApplicationDbContext : DbContext
    {
		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
		   : base(options)
		{
		}

		public DbSet<Product> Products { get; set; }
		public DbSet<CheckProduct> CheckProducts { get; set; }
		public DbSet<CheckData> Checks { get; set; }
	}
}
