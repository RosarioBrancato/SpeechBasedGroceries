using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Product
	{
		public enum QtyTypes
		{
			gramm,
			milliliter,
			pieces
		}

		public string Id { get; set; }

		public string Name { get; set; }

		public string Barcode { get; set; }

		public double Qty { get; set; }

		public QtyTypes Qty_type { get; set; }

		public IList<NutritionValue> NutritionValues { get; }


		public Product()
		{
			this.NutritionValues = new List<NutritionValue>();
		}
		protected Product(Product p)
		{
			this.Id = p.Id;
			this.Name = p.Name;
			this.Barcode = p.Barcode;
			this.NutritionValues = p.NutritionValues;
			this.Qty = p.Qty;
			this.Qty_type = p.Qty_type;
		}
	}
}
