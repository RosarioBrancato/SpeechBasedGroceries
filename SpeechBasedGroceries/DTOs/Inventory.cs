using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Inventory
	{
		public string Fridgename { get; set; }

		public Guid FridgeUUID { get; set; }

		public IList<InventoryItem> Items { get; }

		public Inventory()
		{
			this.Items = new List<InventoryItem>();
		}

	}
}
