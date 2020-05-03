using SpeechBasedGroceries.BusinessLogic.Base;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Fridgy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.BusinessLogic
{
	public class Inspector : CiuBase
	{

		public Inventory GetFridgeInventory()
		{
			Inventory inventory = null;

			if (this.CurrentCustomer != null)
			{
				Token token = this.CurrentCustomer.GetFridigyToken();
				if (token != null && !string.IsNullOrWhiteSpace(token.Value))
				{
					this.FridgyClient.SetToken(token.Value);
					inventory = this.FridgyClient.GetFridgeInventory();
				}
			}

			return inventory;
		}

		public IList<Product> GetItemNutritionalValues(string productName)
		{
			return this.FridgyClient.GetProductsByName(productName);
		}

		public IList<Product> SearchProduct(string QueryTerm)
		{
			// Product contains all nutrient data anyway
			// change to the correct dto
			// return this.fridgyClient.GetProductsByName(QueryTerm);
			return null;
		}

	}
}