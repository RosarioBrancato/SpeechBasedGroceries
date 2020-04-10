using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;
using SpeechBasedGroceries.AppServices;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.CRM
{
    public class CrmDatabase
    {
		private static SqlConnection instance = null;
		public IConfiguration Configuration { get; }
		private static ILogger<CrmDatabase> _logger;
		

		public static SqlConnection Instance
		{
			get
			{   
				if (instance == null)
				{
                    // TODO: how to get configurations differently?
					var configuration = new ConfigurationBuilder()
                        .AddJsonFile("appsettings.json")
                        .Build();

					instance = new CrmDatabase(configuration).connect();
					// _logger = AppLoggerFactory.GetLogger<CrmDatabase>();
				}

			    return instance;
			}
		}

		private CrmDatabase(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private SqlConnection connect()
		{
            string server = Configuration["CRMDB:Server"];
			string port = Configuration["CRMDB:Port"];
			string cat = Configuration["CRMDB:Catalog"];
			string user = Configuration["CRMDB:User"];
			string pw = Configuration["CRMDB:Password"];
            string timeout = Configuration["CRMDB:Timeout"];

			string constr =
                $"Server={server},{port};" +
                $"Initial Catalog={cat};" +
                $"Persist Security Info=False;" +
                $"User ID={user};" +
                $"Password={pw};" +
                $"MultipleActiveResultSets=False;" +
                $"Encrypt=True;" +
                $"TrustServerCertificate=False;" +
                $"Connection Timeout={timeout};";

			return new SqlConnection(constr);
		}
	}
}
