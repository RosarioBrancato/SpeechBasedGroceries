using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceriesTest.Data;

namespace SpeechBasedGroceriesTest
{
	[TestClass]
	public class FridgyClientTest
	{

		private FridgyClient fridgyClient;


		[TestInitialize]
		public void Init()
		{
			string token = UnitTestData.Instance.FridgyToken;
			this.fridgyClient = new FridgyClient(token);
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
	}
}
