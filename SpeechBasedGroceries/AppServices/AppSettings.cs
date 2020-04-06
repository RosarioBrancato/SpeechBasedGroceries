using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.AppServices
{
	public class AppSettings
	{

		private static AppSettings instance = null;

		public static AppSettings Instance {
			get {
				if (instance == null)
				{
					instance = new AppSettings();
				}
				return instance;
			}
		}


		public string FridgyToken { get; private set; }

		private AppSettings()
		{
		}

		public void InitFromConfiguration(IConfiguration configuration)
		{
			this.FridgyToken = configuration["TEMP:FridgyToken"];
		}

	}
}
