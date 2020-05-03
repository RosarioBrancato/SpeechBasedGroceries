using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs.Settings
{
	public class Auth
	{

		public string Domain { get; private set; }

		public string Audience { get; private set; }


		public Auth(IConfiguration configuration)
		{
			this.Domain = configuration["Auth0:Domain"];
			this.Audience = configuration["Auth0:Audience"];
		}

	}
}
