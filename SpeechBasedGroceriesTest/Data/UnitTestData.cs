using Newtonsoft.Json.Linq;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceriesTest.DTOs;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace SpeechBasedGroceriesTest.Data
{
	public class UnitTestData
	{

		private static UnitTestData instance = null;

		public static UnitTestData Instance {
			get {
				if (instance == null)
				{
					instance = new UnitTestData();
				}
				return instance;
			}
		}


		public string FridgyToken { get; private set; }

		public List<TestUser> TestUsers { get; private set; }

		public List<Customer> TestCustomers { get; private set; }

		public List<Token> TestTokens { get; private set; }


		private UnitTestData()
		{
			this.TestUsers = new List<TestUser>();
			this.TestCustomers = new List<Customer>();
			this.TestTokens = new List<Token>();
			this.LoadData();
		}


		private void LoadData()
		{
			JObject data = JObject.Parse(File.ReadAllText("Data/UnitTestData.json"));

			// Fridgy
			this.FridgyToken = data["Fridgy"]["Token"].Value<string>();

			var testusers = data["Fridgy"]["Testusers"].ToArray();
			foreach (var testuser in testusers)
			{
				this.TestUsers.Add(testuser.ToObject<TestUser>());
			}

			//Crm testusers
			testusers = data["Crm"]["TestCustomers"].ToArray();
			foreach (var testuser in testusers)
			{
				this.TestCustomers.Add(testuser.ToObject<Customer>());
			}

			//crm test tokens
			var testTokens = data["Crm"]["TestTokens"].ToArray();
			foreach (var testToken in testTokens)
			{
				this.TestTokens.Add(testToken.ToObject<Token>());
			}
		}

	}
}
