using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs.Settings
{
	public abstract class Database
	{

		public string Server { get; protected set; }

		public string Port { get; protected set; }

		public string Catalog { get; protected set; }

		public string User { get; protected set; }

		public string Password { get; protected set; }

		public string Timeout { get; protected set; }


		public Database(IConfiguration configuration)
		{
		}

	}
}
