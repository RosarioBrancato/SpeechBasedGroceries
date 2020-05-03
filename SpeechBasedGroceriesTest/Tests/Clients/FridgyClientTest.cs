using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using SpeechBasedGroceriesTest.Data;
using SpeechBasedGroceriesTest.DTOs;
using SpeechBasedGroceriesTest.Tests.Base;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.IO.MemoryMappedFiles;
using System.Linq;

namespace SpeechBasedGroceriesTest.Tests.Clients
{
	[TestClass]
	public class FridgyClientTest : BaseTest
	{

		private FridgyClient fridgyClient;
		private string userUUID;


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
			TestUser Albert = UnitTestData.Instance.TestUsers.ElementAt(0);
			this.fridgyClient.SetBasicAuth(Albert.Username, Albert.Password);
			string token = this.fridgyClient.RetrieveToken();
			this.fridgyClient.SetToken(token);

			IList<Fridge> fridges = this.fridgyClient.GetFridges();
			Assert.IsTrue(fridges.Count > 0);
		}

		[TestMethod]
		public void TestRegisterDeleteUser()
		{
			string UUID = Guid.NewGuid().ToString();
			var username = "lonelyuser-" + UUID.Substring(0, 4);
			var password = UUID;
			var email = username + "@google.com";
			var displayname = username;


			User user = this.fridgyClient.RegisterUser(username, password, displayname, email);
			userUUID = user.Uuid.ToString();
			Assert.IsNotNull(user);

			// duplicate user
			User DuplicateUser = this.fridgyClient.RegisterUser(username, password, displayname, email);
			Assert.IsNull(DuplicateUser);

			// delete user again
			string token = UnitTestData.Instance.FridgyToken;
			this.fridgyClient.SetToken(token);
			Assert.IsNotNull(userUUID);
			this.fridgyClient.DeleteUser(userUUID);
		}

		[TestMethod]
		public void TestRetrieveToken()
		{
			TestUser Albert = UnitTestData.Instance.TestUsers.ElementAt(0);
			this.fridgyClient.SetBasicAuth(Albert.Username, Albert.Password);
			string token = this.fridgyClient.RetrieveToken();

			Assert.IsNotNull(token);

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token);

			Assert.IsTrue(tokenHandler.CanReadToken(token));
			Assert.AreEqual(default, jwtToken.ValidTo);
		}

		[TestMethod()]
		public void GetProductsByBarcodeTest()
		{
			Product p = this.fridgyClient.GetProductsByBarcode("4099200025162");
			Assert.AreEqual(p.Name, "Bar Italia Kaffeekapseln");
		}

		[TestMethod()]
		public void GetItemsTest()
		{
			TestUser Albert = UnitTestData.Instance.TestUsers.ElementAt(0);
			this.fridgyClient.SetBasicAuth(Albert.Username, Albert.Password);
			string token = this.fridgyClient.RetrieveToken();
			this.fridgyClient.SetToken(token);
			Fridge fridge = this.fridgyClient.GetFridges().First();

			IList<Item> items = fridgyClient.GetItems(fridge.Id.ToString());
			Assert.IsTrue(items.Count > 0);
		}

		[TestMethod()]
		public void CreateNewFridgeTest()
		{
			TestUser Bertha = UnitTestData.Instance.TestUsers.ElementAt(1);
			this.fridgyClient.SetBasicAuth(Bertha.Username, Bertha.Password);
			string token = this.fridgyClient.RetrieveToken();
			this.fridgyClient.SetToken(token);

			string fridgeName = "dummyfridge DELETE";
			Fridge fridge = this.fridgyClient.CreateNewFridge(fridgeName);

			Assert.IsNotNull(fridge);
			Assert.AreEqual(fridge.Name, fridgeName);

			IList<Fridge> fridges = this.fridgyClient.GetFridges();
			Assert.IsTrue(fridges.Count > 0);
		}

		[TestMethod()]
		public void AddRemoveUserToFridgeTest()
		{
			// Define participants
			TestUser Albert = UnitTestData.Instance.TestUsers.ElementAt(0);
			TestUser Bertha = UnitTestData.Instance.TestUsers.ElementAt(1);

			// Login Bertha
			this.fridgyClient.SetBasicAuth(Bertha.Username, Bertha.Password);
			string BerthaToken = this.fridgyClient.RetrieveToken();
			this.fridgyClient.SetToken(BerthaToken);

			// Login Albert
			this.fridgyClient.SetBasicAuth(Albert.Username, Albert.Password);
			string AlbertToken = this.fridgyClient.RetrieveToken();
			this.fridgyClient.SetToken(AlbertToken);

			// Get Alberts fridge
			Fridge AlbertFridge = this.fridgyClient.GetFridges().First();

			// Check Bertha has no access yet
			this.fridgyClient.SetToken(BerthaToken);
			IList<Fridge> BerthaFridges = this.fridgyClient.GetFridges();
			Assert.IsFalse(BerthaFridges.Contains(AlbertFridge));


			// Alberts adds Bertha to fridge
			this.fridgyClient.SetToken(AlbertToken);
			this.fridgyClient.AddUserToFridge(Bertha.UUID, AlbertFridge.Id.ToString());

			// Bertha should now see Alberts Fridge
			this.fridgyClient.SetToken(BerthaToken);
			BerthaFridges = this.fridgyClient.GetFridges();
			Assert.IsTrue(BerthaFridges.Contains(AlbertFridge));

			// Albert removes her from the fridge again
			this.fridgyClient.SetToken(AlbertToken);
			this.fridgyClient.RemoveUserFromFridge(Bertha.UUID, AlbertFridge.Id.ToString());

			// Check Bertha has no access anymore
			this.fridgyClient.SetToken(BerthaToken);
			BerthaFridges = this.fridgyClient.GetFridges();
			Assert.IsFalse(BerthaFridges.Contains(AlbertFridge));
		}
	}
}
