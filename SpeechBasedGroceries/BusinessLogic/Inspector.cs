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

		public IList<Product> SearchProduct(string QueryTerm) {
			// Product contains all nutrient data anyway
			return this.fridgyClient.GetProductsByName(QueryTerm);
		}

	}
}