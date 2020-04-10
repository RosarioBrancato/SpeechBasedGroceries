using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using Microsoft.Extensions.Logging;

namespace SpeechBasedGroceries.Parties.CRM
{
    public class CrmDAO
    {

		private readonly ILogger<CrmClient> _logger;
		public IConfiguration configuration { get; }



		public CrmDAO()
		{
			this._logger = AppLoggerFactory.GetLogger<CrmClient>();

			// TODO: how to get configurations differently?
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();
            
			this.configuration = configuration;
		}


        
		public List<Customer> GetCustomers()
		{
            // construct statement
            string sql =
				"SELECT c.f_cus_id, c.f_cus_clientno, c.f_cus_firstname, c.f_cus_surname " +
				"FROM [dbo].[t_customer] as c " +
				"ORDER BY c.f_cus_surname";

            // instanciate return object
			List<Customer> customers = new List<Customer>();
            
            // access the database
			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								Customer c = MappingProfile(reader);

								customers.Add(c);
								_logger.LogInformation("received DB result: " + c.toString());
							}
						}
					} // reader closed and disposed up here
				} // command disposed here
			} //connection closed and disposed here

			return customers;
		}


		public Customer GetCustomerByClientNo(int clientno)
		{
			string sql =
				"SELECT c.f_cus_id, c.f_cus_clientno, c.f_cus_firstname, c.f_cus_surname " +
				"FROM [dbo].[t_customer] as c " +
				"WHERE c.f_cus_clientno = (@p1)";

			Customer c = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", clientno);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								//should only have 1 record
								c = MappingProfile(reader);
							}
						}
					}
				} 
			}
      
			return c;
		}



        private Customer MappingProfile(SqlDataReader dr)
        {
			Customer customer = new Customer()
			{
				Id = Int32.Parse(dr["f_cus_id"].ToString()),
				ClientNo = Int32.Parse(dr["f_cus_clientno"].ToString()),
				Firstname = dr["f_cus_firstname"].ToString(),
				Surname = dr["f_cus_surname"].ToString()
			};
			return customer;
		}


		private SqlConnection getConnection()

		{
			string server = configuration["CRMDB:Server"];
			string port = configuration["CRMDB:Port"];
			string cat = configuration["CRMDB:Catalog"];
			string user = configuration["CRMDB:User"];
			string pw = configuration["CRMDB:Password"];
			string timeout = configuration["CRMDB:Timeout"];

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
