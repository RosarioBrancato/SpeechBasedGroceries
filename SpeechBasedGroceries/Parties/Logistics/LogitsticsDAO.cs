using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using System.Data.SqlClient;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using Microsoft.Extensions.Logging;

namespace SpeechBasedGroceries.Parties.Logistics
{
    public class LogisticsDAO
    {

		private readonly ILogger<LogisticsClient> _logger;
		public IConfiguration configuration { get; }



		public LogisticsDAO()
		{
			this._logger = AppLoggerFactory.GetLogger<LogisticsClient>();

			// TODO: how to get configurations differently?
			var configuration = new ConfigurationBuilder()
				.AddJsonFile("appsettings.json")
				.Build();
            
			this.configuration = configuration;
		}



		#region delivery queries

		public List<Delivery> GetDeliveries()
		{
            string sql =
				GetSelectAllDeliveriesStatement()
                + "ORDER BY d.f_del_date DESC";

			List<Delivery> deliveries = new List<Delivery>();
            
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
								Delivery delivery = MappingDelivery(reader);

								deliveries.Add(delivery);
								_logger.LogInformation("received DB result: " + delivery.toString());
							}
						}
					}
				}
			}

			return deliveries;
		}


		[Obsolete("not a use case")]
		public List<Delivery> GetDeliveriesByQuery(int? customerno, DateTime? date)
		{
			string sql =
				GetSelectAllDeliveriesStatement()
				+ "WHERE "
				+ ((customerno != null) ? "d.f_del_customerid = (@p1) " : "")
				+ ((date != null) ? "AND d.f_del_date = (@p2) " : "") ;
			
			List<Delivery> deliveries = new List<Delivery>();

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					if (customerno != null) {
						cmd.Parameters.AddWithValue("@p1", customerno);
					}
                    if (date != null)
                    {
						cmd.Parameters.AddWithValue("@p2", (DateTime)date);
					}
					//cmd.Parameters.AddWithValue("@p2", ((DateTime)date).ToString("yyyy-MM-dd"));

					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								Delivery delivery = MappingDelivery(reader);

								deliveries.Add(delivery);
								_logger.LogInformation("received DB result: " + delivery.toString());
							}
						}
					}
				}
			}

			return deliveries;
		}

		public Delivery GetDeliveryById(int deliveryId)
		{
			string sql =
				GetSelectAllDeliveriesStatement()
				+ "WHERE d.f_del_id = (@p1)";

			Delivery delivery = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", deliveryId);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								//should only have 1 record
								delivery = MappingDelivery(reader);
							}
						}
					}
				} 
			}
      
			return delivery;
		}

		public List<Delivery> GetDeliveriesByCustomerId(int customerId)
		{
			string sql =
				GetSelectAllDeliveriesStatement()
				+ "WHERE d.f_del_customerid = (@p1)";

			List<Delivery> deliveries = new List<Delivery>();

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
								Delivery delivery = MappingDelivery(reader);

								deliveries.Add(delivery);
								_logger.LogInformation("received DB result: " + delivery.toString());
							}
						} else
                        {
							_logger.LogInformation($"customer «{customerId}» does not have any deliveries yet");
						}
					}
				}
			}

			return deliveries;
		}

		public Delivery CheckIfDeliveryBlongsToCustomer(int customerId, int deliveryId)
		{
			string sql =
				GetSelectAllDeliveriesStatement()
				+ "WHERE d.f_del_customerid = (@p1) "
                + "  AND d.f_del_id = (@p2) ";

			Delivery delivery = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", customerId);
					cmd.Parameters.AddWithValue("@p2", deliveryId);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
                                // should only have 1 record
                                delivery = MappingDelivery(reader);
								_logger.LogInformation("received DB result: " + delivery.toString());
							}
						}
						else
						{
							_logger.LogInformation($"customer «{customerId}» does not have a delivery with ID «{deliveryId}»");
						}
					}
				}
			}

			return delivery;
		}



		private string GetSelectAllDeliveriesStatement()
		{
			return "SELECT d.f_del_id, d.f_del_customerid, d.f_del_date, "
				+ "       d.f_del_street, d.f_del_zip, d.f_del_city, d.f_del_country, "
				+ "       d.f_del_comment "
				+ "FROM t_delivery as d ";

		}

		#endregion




		#region position queries

		private List<Position> GetPositions(int deliveryId)
        {
            string sql =
                "SELECT p.f_pos_id, p.f_del_id, p.f_pos_no, "
                + "       p.f_pos_itemid, p.f_pos_itemtext, p.f_pos_itemqty, p.f_pos_itemprice, p.f_pos_itemweight, "
                + "       p.f_pos_comment "
                + "FROM t_position as p "
                + "WHERE p.f_del_id = (@p1)";

            List<Position> positions = new List<Position>();

            using (SqlConnection connection = this.getConnection())
            {
                connection.Open();
                using (SqlCommand cmd = new SqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@p1", deliveryId);
                    using (SqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader != null)
                        {
                            while (reader.Read())
                            {
                                //should only have 1 record
                                Position position = MappingPosition(reader);
                                positions.Add(position);
                            }
                        }
                    }
                }
            }

            return positions;
        }

        #endregion







        #region mapping profiles

        private Delivery MappingDelivery(SqlDataReader dr)
        {
            Delivery delivery = new Delivery()
            {
                Id = Int32.Parse(dr["f_del_id"].ToString()),
                CustomerId = dr.IsDBNull("f_del_customerid") ? default(int) : Int32.Parse(dr["f_del_customerid"].ToString()),
                Date = dr.IsDBNull("f_del_date") ? default(DateTime) : DateTime.Parse(dr["f_del_date"].ToString()),
                Street = dr.IsDBNull("f_del_street") ? default(string) : dr["f_del_street"].ToString(),
                Zip = dr.IsDBNull("f_del_zip") ? default(string) : dr["f_del_zip"].ToString(),
                City = dr.IsDBNull("f_del_city") ? default(string) : dr["f_del_city"].ToString(),
                Country = dr.IsDBNull("f_del_country") ? default(string) : dr["f_del_country"].ToString(),
                Comment = dr.IsDBNull("f_del_comment") ? default(string) : dr["f_del_comment"].ToString(),
            };
            delivery.Positions = GetPositions(delivery.Id);
            return delivery;
        }

        private Position MappingPosition(SqlDataReader dr)
        {
            Position position = new Position()
            {
                Id = Int32.Parse(dr["f_pos_id"].ToString()),
				DeliveryId = Int32.Parse(dr["f_del_id"].ToString()),
				No = dr.IsDBNull("f_del_comment") ? default(int) : Int32.Parse(dr["f_pos_no"].ToString()),
                ItemId = dr.IsDBNull("f_pos_itemid") ? default(string) : dr["f_pos_itemid"].ToString(),
                ItemText = dr.IsDBNull("f_pos_itemtext") ? default(string) : dr["f_pos_itemtext"].ToString(),
                ItemQty = dr.IsDBNull("f_pos_itemqty") ? default(int) : Int32.Parse(dr["f_pos_itemqty"].ToString()),
                ItemPrice = dr.IsDBNull("f_pos_itemprice") ? default(double) : Double.Parse(dr["f_pos_itemprice"].ToString()),
                ItemWeight = dr.IsDBNull("f_pos_itemweight") ? default(double) : Double.Parse(dr["f_pos_itemweight"].ToString()),
                Comment = dr.IsDBNull("f_pos_comment") ? default(string) : dr["f_pos_comment"].ToString()
            };
            return position;
        }

        #endregion







        #region db connection

        private SqlConnection getConnection()

		{
			string server = configuration["LogisticsDB:Server"];
			string port = configuration["LogisticsDB:Port"];
			string cat = configuration["LogisticsDB:Catalog"];
			string user = configuration["LogisticsDB:User"];
			string pw = configuration["LogisticsDB:Password"];
			string timeout = configuration["LogisticsDB:Timeout"];

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
