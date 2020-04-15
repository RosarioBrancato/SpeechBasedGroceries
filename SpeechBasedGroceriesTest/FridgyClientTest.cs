using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using SpeechBasedGroceriesTest.Data;
using System;
using System.Collections.Generic;

namespace SpeechBasedGroceriesTest
{
	[TestClass]
	public class FridgyClientTest
	{

		private FridgyClient fridgyClient;
		private string UserUUID;


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

		[TestMethod]
		public void TestRegisterDeleteUser()
		{
			String UUID = Guid.NewGuid().ToString();
			var username = "lonelyuser-" + UUID.Substring(0, 4);
			var password = UUID;
			var email = username + "@google.com";
			var displayname = username;
			

			User user = this.fridgyClient.RegisterUser(username, password, displayname, email);
			UserUUID = user.Uuid.ToString();
			Assert.IsNotNull(user);

			// duplicate user
			User DuplicateUser = this.fridgyClient.RegisterUser(username, password, displayname, email);
			Assert.IsNull(DuplicateUser);

			// delete user again
			string token = UnitTestData.Instance.FridgyToken;
			this.fridgyClient.Token = token;
			Assert.IsNotNull(UserUUID);
			this.fridgyClient.DeleteUser(UserUUID);
		}

		



	}
}
