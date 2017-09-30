using CashRegister.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
