using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.BusinessLogic
{
	public class Orderer
	{

		public bool LoginWithTelegram(string telegramId)
		{
			//TO-DO
			return false;
		}

		public bool PlaceOrder()
		{
			//TO-DO

			//CRM: get user personal info (name, address etc.)
			//BANK: check balance
			//BANK: edit balance
			//LOG: place order in logistics
			//FRIDGY: place items in fridge
			//BOT: return confirmation

			return true;
		}

	}
}
