using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Npgsql;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using Microsoft.Extensions.Logging;

namespace SpeechBasedGroceries.Parties.CRM
{
    public class CrmDAO
    {

		private readonly ILogger<CrmClient> _logger;


		public CrmDAO()
		{
			_logger = AppLoggerFactory.GetLogger<CrmClient>();
		}

		public List<Customer> GetCustomers()
		{

			string sql =
                "SELECT f_cli_id, f_cli_clientno, f_cli_firstname, f_cli_surname " +
                "FROM t_client" +
                "ORDER BY f_cli_surname";

			NpgsqlCommand cmd = new NpgsqlCommand(sql, CrmDatabase.Instance);
			NpgsqlDataReader dr = cmd.ExecuteReader();

			List<Customer> returns = null;
            while (dr.Read())
            {
				Customer c = MappingProfile(dr);

				returns.Add(c);
				_logger.LogInformation("received DB result: " + c.toString());
            }

			return returns;
		}


		public Customer GetCustomerByClientNo(int clientno)
		{

			string sql =
				"SELECT f_cli_id, f_cli_clientno, f_cli_firstname, f_cli_surname " +
				"FROM t_client" +
				"WHERE f_cli_clientno = (@p)";

			NpgsqlCommand cmd = new NpgsqlCommand(sql, CrmDatabase.Instance);
			cmd.Parameters.AddWithValue("p", clientno);
			NpgsqlDataReader dr = cmd.ExecuteReader();

			Customer c = null;
			while (dr.Read())
			{
				//should only have 1 record
				c = MappingProfile(dr);
			}

			return c;
		}


        private Customer MappingProfile(NpgsqlDataReader dr)
        {
			Customer customer = new Customer()
			{
				Id = Int32.Parse(dr["f_cli_id"].ToString()),
				ClientNo = Int32.Parse(dr["f_cli_clientno"].ToString()),
				Firstname = dr["f_cli_firstname"].ToString(),
				Surname = dr["f_cli_surname"].ToString()
			};
			return customer;
		}

	}
}
