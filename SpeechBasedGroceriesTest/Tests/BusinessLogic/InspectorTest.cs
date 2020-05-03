using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceriesTest.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeechBasedGroceriesTest.Tests.BusinessLogic
{
	[TestClass()]
	public class InspectorTest : BaseTest
	{

		private Inspector inspector;

		[TestInitialize]
		public void Init()
		{
			this.inspector = new Inspector();
		}

		[TestCleanup]
		public void Cleanup()
		{
			this.inspector = null;
		}

		[TestMethod()]
		public void TestGetFridgeInventory()
		{
			Random random = new Random();

			string uuid = Guid.NewGuid().ToString().Substring(0, 4);

			TelegramUser telegramUser = new TelegramUser();
			telegramUser.Id = random.Next(100000000, 999999999);
			telegramUser.FirstName = "Max-" + uuid;
			telegramUser.LastName = "Muster-" + uuid;

			inspector.LoginWithTelegram(telegramUser);
			Inventory inv = inspector.GetFridgeInventory();
			Assert.IsNotNull(inv);
		}
	}
}