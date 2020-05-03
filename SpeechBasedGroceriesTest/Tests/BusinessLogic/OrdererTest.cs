using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Logistics;
using SpeechBasedGroceriesTest.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeechBasedGroceriesTest.Tests.BusinessLogic
{
	[TestClass()]
	public class OrdererTest : BaseTest
	{
		private Orderer oderer;

		[TestInitialize]
		public void Init()
		{
			string uuid = Guid.NewGuid().ToString().Substring(0, 4);

			TelegramUser telegramUser = new TelegramUser();
			telegramUser.Id = 356131678;
			telegramUser.FirstName = "Max-" + uuid;
			telegramUser.LastName = "Muster-" + uuid;

			this.oderer = new Orderer();
			this.oderer.LoginWithTelegram(telegramUser);
		}

		[TestCleanup]
		public void Cleanup()
		{
			this.oderer = null;
		}

		[TestMethod()]
		public void TestPlaceOrder()
		{
			string barcode = "7610200428059"; // Bratwurst

			Product product = new FridgyClient().GetProductByBarcode(barcode);
			Delivery delivery = oderer.PlaceOrder(product, 1, "Eine Bestellung");

			Assert.IsNotNull(delivery);
			Assert.IsTrue(delivery.Positions[0].ItemId.Equals(barcode));
		}
	}
}