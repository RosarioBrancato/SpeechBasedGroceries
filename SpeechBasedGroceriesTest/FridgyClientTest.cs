using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using SpeechBasedGroceriesTest.Data;
using System.Collections.Generic;

namespace SpeechBasedGroceriesTest
{
	[TestClass]
	public class FridgyClientTest
	{

		private FridgyClient fridgyClient;


		[TestInitialize]
		public void Init()
		{
			this.fridgyClient = new FridgyClient();
		}

		[TestCleanup]
		public void Cleanup()
		{
			this.fridgyClient = null;
		}


		[TestMethod]
		public void TestGetProductsByName()
		{
			string productName = "Milch";
			var products = this.fridgyClient.GetProductsByName(productName);

			Assert.IsTrue(products.Count > 0);
		}

		[TestMethod]
		public void TestGetFridges()
		{
			string token = UnitTestData.Instance.FridgyToken;
			this.fridgyClient.Token = token;

			IList<Fridge> fridges = this.fridgyClient.GetFridges();
			Assert.IsTrue(fridges.Count > 0);
		}

		
	}
}
