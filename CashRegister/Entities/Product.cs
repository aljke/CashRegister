using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CashRegister.Entities
{
	public class Product
    {
		[Key]
		public int Id { get; set; }
		public string Caption { get; set; }
		public virtual ICollection<CheckProduct> ThisCheckProducts { get; set; }
    }
}
