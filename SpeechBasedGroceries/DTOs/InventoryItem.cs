using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class InventoryItem : Product
	{

		public InventoryItem(Product obj)
		 : base(obj)
		{
		}

		public double Quantity { get; set; }

	}
}
