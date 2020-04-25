using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs
{
	public class Product
	{

		public string Id { get; set; }

		public string Name { get; set; }

		public string Barcode { get; set; }

		public IList<NutritionValue> NutritionValues { get; }


		public Product()
		{
			this.NutritionValues = new List<NutritionValue>();
		}
	}
}
