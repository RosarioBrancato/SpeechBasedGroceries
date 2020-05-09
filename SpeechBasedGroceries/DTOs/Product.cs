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

		public string ImageUrl { get; set; }

		public double Qty { get; set; }

		public QtyTypes QtyType { get; set; }

		public IList<NutritionValue> NutritionValues { get; }


		public Product()
		{
			this.NutritionValues = new List<NutritionValue>();
		}

		protected Product(Product p)
		{
			this.Id = p.Id;
			this.Name = p.Name.Trim();
			this.Barcode = p.Barcode;
			this.ImageUrl = p.ImageUrl;
			this.NutritionValues = p.NutritionValues;
			this.Qty = p.Qty;
			this.QtyType = p.QtyType;
		}
	}
}
