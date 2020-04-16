using Newtonsoft.Json.Linq;
using SpeechBasedGroceries.DTOs;
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
		public IList<TestUser> Testusers { get; private set; }
		public IList<Customer> TestCustomers { get; private set; }
		public IList<Token> TestTokens { get; private set; }

		private UnitTestData()
		{
			Testusers = new List<TestUser>();
			TestCustomers = new List<Customer>();
			TestTokens = new List<Token>();
			this.LoadData();
		}

		private void LoadData()
		{
			JObject data = JObject.Parse(File.ReadAllText("Data/UnitTestData.json"));


            // Fridgy
			this.FridgyToken = data["Fridgy"]["Token"].Value<string>();

			var resultObjects = AllChildren(data)
			   .First(c => c.Type == JTokenType.Array && c.Path.Contains("Testusers"))
			   .Children<JObject>();

			foreach (JObject result in resultObjects)
			{
				TestUser u = result.ToObject<TestUser>();
				this.Testusers.Add(u);
			}



            //Crm
			resultObjects = AllChildren(data)
			   .First(c => c.Type == JTokenType.Array && c.Path.Contains("TestCustomers"))
			   .Children<JObject>();

			foreach (JObject result in resultObjects)
			{
				Customer c = result.ToObject<Customer>();
				this.TestCustomers.Add(c);
			}
			resultObjects = AllChildren(data)
			   .First(c => c.Type == JTokenType.Array && c.Path.Contains("TestTokens"))
			   .Children<JObject>();

			foreach (JObject result in resultObjects)
			{
				Token t = result.ToObject<Token>();
				this.TestTokens.Add(t);
			}


		}

		// recursively yield all children of json
		private static IEnumerable<JToken> AllChildren(JToken json)
		{
			foreach (var c in json.Children())
			{
				yield return c;
				foreach (var cc in AllChildren(c))
				{
					yield return cc;
				}
			}
		}

	}
}
