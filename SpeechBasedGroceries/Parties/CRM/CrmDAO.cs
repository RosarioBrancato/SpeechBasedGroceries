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


        #region customer queries

        public List<Customer> GetCustomers()
		{
            // construct statement
            string sql =
				GetSelectAllStatement()
                + "ORDER BY c.f_cus_surname";

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
								Customer c = MappingCustomer(reader);

								customers.Add(c);
								_logger.LogInformation("received DB result: " + c.toString());
							}
						}
					} // reader closed and disposed up here
				} // command disposed here
			} //connection closed and disposed here

			return customers;
		}


		public Customer GetCustomerByNo(int customerNo)
		{
			string sql =
                GetSelectAllStatement()
                + "WHERE c.f_cus_no = (@p1)";

			Customer c = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", customerNo);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								//should only have 1 record
								c = MappingCustomer(reader);
							}
						}
					}
				} 
			}
      
			return c;
		}


		private string GetSelectAllStatement()
        {
			return "SELECT c.f_cus_id, c.f_cus_no, c.f_cus_firstname, c.f_cus_surname, c.f_cus_birthdate, "
                +  "       c.f_cus_street, c.f_cus_zip, c.f_cus_city, c.f_cus_country, "
                +  "       c.f_cus_email "
                +  "FROM [dbo].[t_customer] as c ";

		}

		#endregion


        #region mapping profiles

		private Customer MappingCustomer(SqlDataReader dr)
        {
			Customer customer = new Customer()
			{
				Id = Int32.Parse(dr["f_cus_id"].ToString()),
				No = Int32.Parse(dr["f_cus_no"].ToString()),
				Firstname = dr["f_cus_firstname"].ToString(),
				Surname = dr["f_cus_surname"].ToString(),
				Birthdate = DateTime.Parse(dr["f_cus_birthdate"].ToString()),
				Street = dr["f_cus_street"].ToString(),
				Zip = dr["f_cus_zip"].ToString(),
				City = dr["f_cus_city"].ToString(),
				Country = dr["f_cus_country"].ToString(),
				Email = dr["f_cus_email"].ToString()
			};
			return customer;
		}

		#endregion




		#region db connection

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

		#endregion


	}
}
