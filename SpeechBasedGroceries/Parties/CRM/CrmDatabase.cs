using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;

namespace SpeechBasedGroceries.Parties.CRM
{
    public class CrmDatabase
    {
		private static NpgsqlConnection instance = null;
		public IConfiguration Configuration { get; }

		public static NpgsqlConnection Instance
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
				}
				return instance;
			}
		}

		private CrmDatabase(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		private NpgsqlConnection connect()
		{
            string server = Configuration["CRM_DB:Server"];
			string port = Configuration["CRM_DB:Port"];
			string user = Configuration["CRM_DB:User"];
			string pw = Configuration["CRM_DB:Password"];
			string db = Configuration["CRM_DB:Database"];

			string constr = $"Server={server}; Port={port}; User Id={user}; Password={pw}; Database={db};";
			Console.WriteLine($"Trying to connect to: {constr}");
			NpgsqlConnection connection = new NpgsqlConnection(constr);
			connection.Open();
			//connection.Close();

			return connection;
		}
	}
}
