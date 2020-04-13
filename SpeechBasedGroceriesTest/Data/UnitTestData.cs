using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
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


		private UnitTestData()
		{
			this.LoadData();
		}

		private void LoadData()
		{
			JObject data = JObject.Parse(File.ReadAllText("Data/UnitTestData.json"));

			this.FridgyToken = data["Fridgy"]["Token"].Value<string>();
		}

	}
}
