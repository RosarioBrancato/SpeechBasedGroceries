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

		public bool LoginWithTelegram(string telegramId)
		{
			//TO-DO
			return false;
		}

		public IList<Item> GetFridgeInventory(string fridgeId, int sortType)
		{
			//TO-DO
			return null;
		}

		public IList<BaseProductNutrient> GetItemNutrientValues(string itemId)
		{
			// TO-DO
			return null;
		}

	}
}
