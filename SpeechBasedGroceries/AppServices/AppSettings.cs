using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using SpeechBasedGroceries.DTOs.Settings;
using System;
using System.Collections.Generic;
using System.IO;
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


		public Auth Auth { get; private set; }

		public CrmDb CrmDb { get; private set; }

		public LogisticsDb LogisticsDb { get; private set; }


		private AppSettings()
		{
		}


		public void InitFromConfiguration(IConfiguration configuration)
		{
			this.Auth = new Auth(configuration);
			this.CrmDb = new CrmDb(configuration);
			this.LogisticsDb = new LogisticsDb(configuration);
		}

		public void InitFromJsonFile()
		{
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();

			this.InitFromConfiguration(configuration);
		}

	}
}
