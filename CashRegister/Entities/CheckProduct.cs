using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CashRegister.Entities
{
	public class CheckProduct
	{
		[Key]
		public int Id { get; set; }
		public int CheckId { get; set; }
		public int ProductId { get; set; }
		public decimal Price { get; set; }
		public int Amount { get; set; }
		[ForeignKey("ProductId")]
		public virtual Product Product { get; set; }
		[ForeignKey("CheckId")]
		public virtual CheckData Check { get; set; }
    }
}
