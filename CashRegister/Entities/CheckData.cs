using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashRegister.Entities
{
	public class CheckData
    {
		[Key]
		public int Id { get; set; }
		public DateTime UtcTime { get; set; }
		public virtual ICollection<CheckProduct> CheckProducts { get; set; }
    }
}
