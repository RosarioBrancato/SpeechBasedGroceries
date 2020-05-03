using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.Bank;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Logistics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.BusinessLogic.Base
{
	public abstract class CiuBase
	{

		protected Customer CurrentCustomer { get; private set; }


		protected CrmClient CrmClient { get; private set; }

		protected FridgyClient FridgyClient { get; private set; }

		protected BankClient BankClient { get; private set; }

		protected LogisticsClient LogisticsClient { get; private set; }


		public CiuBase()
		{
			this.CrmClient = new CrmClient();
			this.FridgyClient = new FridgyClient();
			this.BankClient = new BankClient();
			this.LogisticsClient = new LogisticsClient();
		}


		public void LoginWithTelegram(TelegramUser telegramUser)
		{
			this.CurrentCustomer = this.CrmClient.GetCustomerByTelegramId(telegramUser.Id);

			if (this.CurrentCustomer == null)
			{
				Registrar registrar = new Registrar();
				this.CurrentCustomer = registrar.RegisterTelegramUser(telegramUser);
			}
		}

	}
}
