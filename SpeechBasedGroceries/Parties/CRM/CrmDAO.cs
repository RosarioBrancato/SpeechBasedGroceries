using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.VisualBasic.CompilerServices;

namespace SpeechBasedGroceries.Parties.CRM
{
    public class CrmDAO
    {

		// private readonly ILogger<CrmDAO> _logger;
		public IConfiguration configuration { get; }



		public CrmDAO()
		{
			// this._logger = AppLoggerFactory.GetLogger<CrmDAO>();
			
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
				GetSelectAllCustomerStatement()
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
							}
						}
					} // reader closed and disposed up here
				} // command disposed here
			} //connection closed and disposed here

			return customers;
		}


		public Customer GetCustomerById(int customerId)
		{
			string sql =
				GetSelectAllCustomerStatement()
                + "WHERE c.f_cus_id = (@p1)";

			Customer customer = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", customerId);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								//should only have 1 record
								customer = MappingCustomer(reader);
							}
						}
					}
				} 
			}
      
			return customer;
		}


		public Customer GetCustomerByTelegramId(int telegramId)
		{
			string sql =
				GetSelectAllCustomerStatement()
				+ "WHERE c.f_cus_telegramid = (@p1)";

			Customer c = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", telegramId);
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
        

		private string GetSelectAllCustomerStatement()
        {
			return "SELECT c.f_cus_id, c.f_cus_firstname, c.f_cus_surname, c.f_cus_birthdate, "
                +  "       c.f_cus_street, c.f_cus_zip, c.f_cus_city, c.f_cus_country, "
                +  "       c.f_cus_email, c.f_cus_telegramid "
                +  "FROM [dbo].[t_customer] as c ";

		}


		public Customer UpdateCustomer(Customer customer)
		{
			string sql = "UPDATE [dbo].[t_customer] "
                       + "SET f_cus_firstname = (@p1), "
					   + "    f_cus_surname = (@p2), "
					   + "    f_cus_birthdate = (@p3), "
					   + "    f_cus_street = (@p4), "
					   + "    f_cus_zip = (@p5), "
					   + "    f_cus_city = (@p6), "
					   + "    f_cus_country = (@p7), "
					   + "    f_cus_email = (@p8), "
					   + "    f_cus_telegramid = (@p9) "
                       + "WHERE f_cus_id = (@p0)";

			Customer _customer = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", (customer.Firstname is null) ? (object)DBNull.Value : customer.Firstname);
					cmd.Parameters.AddWithValue("@p2", (customer.Surname is null) ? (object)DBNull.Value : customer.Surname);
					cmd.Parameters.AddWithValue("@p3", (customer.Birthdate is null) ? (object)DBNull.Value : customer.Birthdate);
					cmd.Parameters.AddWithValue("@p4", (customer.Street is null) ? (object)DBNull.Value : customer.Street);
					cmd.Parameters.AddWithValue("@p5", (customer.Zip is null) ? (object)DBNull.Value : customer.Zip);
					cmd.Parameters.AddWithValue("@p6", (customer.City is null) ? (object)DBNull.Value : customer.City);
					cmd.Parameters.AddWithValue("@p7", (customer.Country is null) ? (object)DBNull.Value : customer.Country);
					cmd.Parameters.AddWithValue("@p8", (customer.Email is null) ? (object)DBNull.Value : customer.Email);
					cmd.Parameters.AddWithValue("@p9", customer.TelegramId);
					cmd.Parameters.AddWithValue("@p0", customer.Id);
					if (cmd.ExecuteNonQuery() == 1)
					{
						_customer = customer;
					};
				}
			}

			return _customer;
		}

        
		public Customer CreateCustomer(Customer customer)
		{
			string sql = "INSERT INTO [dbo].[t_customer] "
                       + "  (f_cus_firstname, f_cus_surname, f_cus_birthdate, "
				       + "   f_cus_street, f_cus_zip, c.f_cus_city, f_cus_country, "
				       + "   f_cus_email, f_cus_telegramid) "
					   + "VALUES "
                       + "  ((@p1), (@p2), (@p3), "
                       + "   (@p4), (@p5), (@p6), (@p7), "
                       + "   (@p8), (@p9)) "
                       + "SELECT SCOPE_IDENTITY()";

			Customer _customer = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", (customer.Firstname is null) ? (object)DBNull.Value : customer.Firstname);
					cmd.Parameters.AddWithValue("@p2", (customer.Surname is null) ? (object)DBNull.Value : customer.Surname);
					cmd.Parameters.AddWithValue("@p3", (customer.Birthdate is null) ? (object)DBNull.Value : customer.Birthdate);
					cmd.Parameters.AddWithValue("@p4", (customer.Street is null) ? (object)DBNull.Value : customer.Street);
					cmd.Parameters.AddWithValue("@p5", (customer.Zip is null) ? (object)DBNull.Value : customer.Zip);
					cmd.Parameters.AddWithValue("@p6", (customer.City is null) ? (object)DBNull.Value : customer.City);
					cmd.Parameters.AddWithValue("@p7", (customer.Country is null) ? (object)DBNull.Value : customer.Country);
					cmd.Parameters.AddWithValue("@p8", (customer.Email is null) ? (object)DBNull.Value : customer.Email);
					cmd.Parameters.AddWithValue("@p9", customer.TelegramId);

					int customerId = 0;
                    try
                    {
						customerId = Int32.Parse(cmd.ExecuteScalar().ToString());
						customer.Id = customerId;
						_customer = customer;
					} catch (Exception e)
                    {
						Console.Write(e.StackTrace);
                    }
				}
			}

			return _customer;
		}


		public bool DeleteCustomer(Customer customer)
		{
			string sql = "DELETE [dbo].[t_customer] "
					   + "WHERE f_cus_id = (@p0)";

			bool succeeded = false;
			DeleteTokensOfCustomer(customer.Id);

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p0", customer.Id);
					succeeded = cmd.ExecuteNonQuery() == 1;
				}
			}

			return succeeded;
		}


		#endregion










		#region token queries


		private List<Token> GetTokens(int customerId)
		{
			string sql =
				  GetSelectAllTokensStatement()
				+ "WHERE t.f_cus_id = (@p1)";

			List<Token> tokens = new List<Token>();

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", customerId);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								Token token = MappingToken(reader);
								tokens.Add(token);
							}
						}
					}
				}
			}

			return tokens;
		}


		public Token GetTokenById(int tokenId)
        {
			string sql =
				  GetSelectAllTokensStatement()
				+ "WHERE t.f_tok_id = (@p1)";

			Token token = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", tokenId);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								//should only have 1 record
								token = MappingToken(reader);
							}
						}
					}
				}
			}

			return token;
		}


		private string GetSelectAllTokensStatement()
		{
			return "SELECT t.f_tok_id, t.f_cus_id, t.f_tok_creation, "
				+ "        t.f_tok_name, t.f_tok_value, t.f_tok_expiration "
				+ "FROM [dbo].[t_token] as t ";
		}


		public Token UpdateToken(Token token)
		{
			string sql = "UPDATE [dbo].[t_token] "
					   + "SET f_tok_creation = (@p1), "
					   + "    f_tok_name = (@p2), "
					   + "    f_tok_value = (@p3), "
					   + "    f_tok_expiration = (@p4) "
					   + "WHERE f_tok_id = (@p5) "
					   + "  AND f_cus_id = (@p6)";

			Token _token = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", token.Creation);
					cmd.Parameters.AddWithValue("@p2", token.Name);
					cmd.Parameters.AddWithValue("@p3", token.Value);
					cmd.Parameters.AddWithValue("@p4", (token.Expiration is null) ? (Object)DBNull.Value : token.Expiration);
					cmd.Parameters.AddWithValue("@p5", token.Id);
					cmd.Parameters.AddWithValue("@p6", token.CustomerId);

					if (cmd.ExecuteNonQuery() == 1)
					{
						_token = token;
					};
				}
			}

			return _token;
		}


		public Token CreateToken(Token token)
		{
			string sql = "INSERT INTO [dbo].[t_token] "
					   + "  (f_cus_id, f_tok_creation, f_tok_name, f_tok_value, f_tok_expiration) "
					   + "VALUES "
					   + "  ((@p1), (@p2), (@p3), (@p4), (@p5)) "
					   + "SELECT SCOPE_IDENTITY()";

			Token _token = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", token.CustomerId);
					cmd.Parameters.AddWithValue("@p2", token.Creation);
					cmd.Parameters.AddWithValue("@p3", token.Name);
					cmd.Parameters.AddWithValue("@p4", token.Value);
					cmd.Parameters.AddWithValue("@p5", (token.Expiration is null) ? (Object)DBNull.Value : token.Expiration);

					int tokenId = 0;
					try
					{
						tokenId = Int32.Parse(cmd.ExecuteScalar().ToString());
						token.Id = tokenId;
						_token = token;
					}
					catch (Exception e)
					{
						Console.Write(e.StackTrace);
					}
				}
			}

			return _token;
		}

		public bool DeleteToken(Token token)
		{
			string sql = "DELETE [dbo].[t_token] "
					   + "WHERE f_tok_id = (@p0)";

			bool succeeded = false;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p0", token.Id);
					succeeded = cmd.ExecuteNonQuery() == 1;
				}
			}

			return succeeded;
		}


		private int DeleteTokensOfCustomer(int customerId)
		{
			string sql = "DELETE [dbo].[t_token] "
					   + "WHERE f_cus_id = (@p0)";

			int rows = 0;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p0", customerId);
					rows = cmd.ExecuteNonQuery();
				}
			}

			return rows;
		}

		#endregion











		#region mapping profiles

		private Customer MappingCustomer(SqlDataReader dr)
        {
			Customer customer = new Customer()
			{
				Id = Int32.Parse(dr["f_cus_id"].ToString()),
				Firstname = dr.IsDBNull("f_cus_firstname") ? default : dr["f_cus_firstname"].ToString(),
				Surname = dr.IsDBNull("f_cus_surname") ? default : dr["f_cus_surname"].ToString(),
				Birthdate = dr.IsDBNull("f_cus_birthdate") ? default : DateTime.Parse(dr["f_cus_birthdate"].ToString()),
				Street = dr.IsDBNull("f_cus_street") ? default : dr["f_cus_street"].ToString(),
				Zip = dr.IsDBNull("f_cus_zip") ? default : dr["f_cus_zip"].ToString(),
				City = dr.IsDBNull("f_cus_city") ? default : dr["f_cus_city"].ToString(),
				Country = dr.IsDBNull("f_cus_country") ? default : dr["f_cus_country"].ToString(),
				Email = dr.IsDBNull("f_cus_email") ? default : dr["f_cus_email"].ToString(),
                TelegramId = dr.IsDBNull("f_cus_telegramid") ? default : Int32.Parse(dr["f_cus_telegramid"].ToString().Trim())
		    };

			customer.Tokens = GetTokens(customer.Id);

			return customer;
		}


		private Token MappingToken(SqlDataReader dr)
		{
			Token token = new Token()
			{
                Id = Int32.Parse(dr["f_tok_id"].ToString()),
				CustomerId = Int32.Parse(dr["f_cus_id"].ToString()),
				Creation = dr.IsDBNull("f_tok_creation") ? default : DateTime.Parse(dr["f_tok_creation"].ToString()),
				Name = dr.IsDBNull("f_tok_name") ? default : dr["f_tok_name"].ToString(),
				Value = dr.IsDBNull("f_tok_value") ? default : dr["f_tok_value"].ToString(),
				Expiration = dr.IsDBNull("f_tok_expiration") ? default : DateTime.Parse(dr["f_tok_expiration"].ToString())
			};

			return token;
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
