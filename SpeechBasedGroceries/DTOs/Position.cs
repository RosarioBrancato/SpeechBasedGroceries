using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Position
	{

		public int Id { get; set; }

		public int DeliveryId { get; set; }

		public int No { get; set; }

		public string ItemId { get; set; }

		public string ItemText { get; set; }

		public int ItemQty { get; set; }

		public double ItemPrice { get; set; }

		public double ItemWeight { get; set; }

		public string Comment { get; set; }


		public override string ToString()
		{
			return this.No.ToString("000") + ": " + this.ItemId + " " + this.ItemText + ", " + this.ItemQty + "x";
		}

	}
}


