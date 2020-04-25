using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.BusinessLogic
{
	public class Inspector
	{
		private FridgyClient fridgyClient = new FridgyClient();

		public void LoginWithTelegram(string telegramId)
		{
			Registrar authenticator = new Registrar();
			Token token = authenticator.LoginWithTelegram(telegramId);
			this.fridgyClient.setToken(token.Value);
		}

		public Inventory GetFridgeInventory()
		{
			Inventory inv = new Inventory();
			Fridge fridge = this.fridgyClient.GetFridges().First();
			inv.Fridgename = fridge.Name;
			inv.FridgeUUID = fridge.Id;
			IList<Item> items = this.fridgyClient.GetItems(fridge.Id.ToString());
			Console.WriteLine("found: " + items.Count + " items: " + items.ToString());
			return inv;
		}

		public IList<DTOs.Product> SearchProduct(string QueryTerm) {
			// Product contains all nutrient data anyway
			// change to the correct dto
			// return this.fridgyClient.GetProductsByName(QueryTerm);
			return null;
		}
		public DTOs.Product GetItemNutrientValues(string itemId)
		{
			// TO-DO


			//DUMMY
			var product = new DTOs.Product();
			product.Name = "Mozzarella";
			product.NutritionValues.Add(new NutritionValue() { Name = "Energy in kJ", Value = "989" });
			product.NutritionValues.Add(new NutritionValue() { Name = "Energy in kcal", Value = "238" });
			product.NutritionValues.Add(new NutritionValue() { Name = "Fat", Value = "18g" });
			product.NutritionValues.Add(new NutritionValue() { Name = "saturated fatty acids", Value = "13g" });
			product.NutritionValues.Add(new NutritionValue() { Name = "Carbohydrate", Value = "2g" });
			product.NutritionValues.Add(new NutritionValue() { Name = "of which sugar", Value = "1g" });
			product.NutritionValues.Add(new NutritionValue() { Name = "Dietary fiber", Value = "0.00g" });
			product.NutritionValues.Add(new NutritionValue() { Name = "Protein", Value = "17g" });
			product.NutritionValues.Add(new NutritionValue() { Name = "Salt", Value = "0.7g" });

			return product;
		}

	}
}