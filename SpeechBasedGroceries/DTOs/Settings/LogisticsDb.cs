using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs.Settings
{
	public class LogisticsDb : Database
	{

		public LogisticsDb(IConfiguration configuration) : base(configuration)
		{
			this.Server = configuration["LogisticsDB:Server"];
			this.Port = configuration["LogisticsDB:Port"];
			this.Catalog = configuration["LogisticsDB:Catalog"];
			this.User = configuration["LogisticsDB:User"];
			this.Password = configuration["LogisticsDB:Password"];
			this.Timeout = configuration["LogisticsDB:Timeout"];
		}

	}
}
