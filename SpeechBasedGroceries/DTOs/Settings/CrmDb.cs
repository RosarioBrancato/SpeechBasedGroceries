using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.DTOs.Settings
{
	public class CrmDb : Database
	{
		public CrmDb(IConfiguration configuration) : base(configuration)
		{
			this.Server = configuration["CRMDB:Server"];
			this.Port = configuration["CRMDB:Port"];
			this.Catalog = configuration["CRMDB:Catalog"];
			this.User = configuration["CRMDB:User"];
			this.Password = configuration["CRMDB:Password"];
			this.Timeout = configuration["CRMDB:Timeout"];
		}
	}
}
