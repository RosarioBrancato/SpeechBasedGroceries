using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs.Settings
{
	public class Auth
	{

		public string UserName { get; private set; }

		public string Password { get; private set; }


		public Auth(IConfiguration configuration)
		{
			this.UserName = configuration["Auth:UserName"];
			this.Password = configuration["Auth:Password"];
		}

	}
}
