using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using Npgsql;
using System.Threading.Tasks;
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
            
			using (NpgsqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
				{
					using (NpgsqlDataReader reader = cmd.ExecuteReader())
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


		public List<Delivery> GetDeliveriesByQuery(int? customerno, DateTime? date)
		{
			string sql =
				GetSelectAllDeliveriesStatement()
				+ "WHERE "
				+ ((customerno != null) ? "d.f_del_customerno = (@p1) " : "")
				+ ((date != null) ? "AND d.f_del_date = (@p2) " : "") ;
			
			List<Delivery> deliveries = new List<Delivery>();

			using (NpgsqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
				{
					if (customerno != null) {
						cmd.Parameters.AddWithValue("@p1", customerno);
					}
                    if (date != null)
                    {
						cmd.Parameters.AddWithValue("@p2", NpgsqlTypes.NpgsqlDbType.Timestamp, (DateTime)date);
					}
					//cmd.Parameters.AddWithValue("@p2", ((DateTime)date).ToString("yyyy-MM-dd"));

					using (NpgsqlDataReader reader = cmd.ExecuteReader())
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

			using (NpgsqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", deliveryId);
					using (NpgsqlDataReader reader = cmd.ExecuteReader())
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

		public List<Delivery> GetDeliveriesByCustomerNo(int customerNo)
		{
			string sql =
				GetSelectAllDeliveriesStatement()
				+ "WHERE d.f_del_customerno = (@p1)";

			List<Delivery> deliveries = new List<Delivery>();

			using (NpgsqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", customerNo);
					using (NpgsqlDataReader reader = cmd.ExecuteReader())
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
							_logger.LogInformation($"customer «{customerNo}» does not have any deliveries yet");
						}
					}
				}
			}

			return deliveries;
		}

		public Delivery GetDeliveryByCustomerNo(int customerNo, int deliveryId)
		{
			string sql =
				GetSelectAllDeliveriesStatement()
				+ "WHERE d.f_del_customerno = (@p1) "
                + "  AND d.f_del_id = (@p2) ";

			Delivery delivery = null;

			using (NpgsqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", customerNo);
					cmd.Parameters.AddWithValue("@p2", deliveryId);
					using (NpgsqlDataReader reader = cmd.ExecuteReader())
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
							_logger.LogInformation($"customer «{customerNo}» does not have a delivery with ID «{deliveryId}»");
						}
					}
				}
			}

			return delivery;
		}



		private string GetSelectAllDeliveriesStatement()
		{
			return "SELECT d.f_del_id, d.f_del_customerno, d.f_del_date, "
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

            using (NpgsqlConnection connection = this.getConnection())
            {
                connection.Open();
                using (NpgsqlCommand cmd = new NpgsqlCommand(sql, connection))
                {
                    cmd.Parameters.AddWithValue("@p1", deliveryId);
                    using (NpgsqlDataReader reader = cmd.ExecuteReader())
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

        private Delivery MappingDelivery(NpgsqlDataReader dr)
        {
            Delivery delivery = new Delivery()
            {
                Id = Int32.Parse(dr["f_del_id"].ToString()),
                CustomerNo = Int32.Parse(dr["f_del_customerno"].ToString()),
                Date = DateTime.Parse(dr["f_del_date"].ToString()),
                Street = dr["f_del_street"].ToString(),
                Zip = dr["f_del_zip"].ToString(),
                City = dr["f_del_city"].ToString(),
                Country = dr["f_del_country"].ToString(),
                Comment = dr["f_del_comment"].ToString(),
            };
            delivery.Positions = GetPositions(delivery.Id);
            return delivery;
        }

        private Position MappingPosition(NpgsqlDataReader dr)
        {
            Position position = new Position()
            {
                Id = Int32.Parse(dr["f_pos_id"].ToString()),
                No = Int32.Parse(dr["f_pos_no"].ToString()),
                ItemId = dr["f_pos_itemid"].ToString(),
                ItemText = dr["f_pos_itemtext"].ToString(),
                ItemQty = Int32.Parse(dr["f_pos_itemqty"].ToString()),
                ItemPrice = Double.Parse(dr["f_pos_itemprice"].ToString()),
                ItemWeight = Double.Parse(dr["f_pos_itemweight"].ToString()),
                Comment = dr["f_pos_comment"].ToString()
            };
            return position;
        }

        #endregion







        #region db connection

        private NpgsqlConnection getConnection()

		{
			string server = configuration["LogisticsDB:Server"];
			string port = configuration["LogisticsDB:Port"];
			string db = configuration["LogisticsDB:Database"];
			string user = configuration["LogisticsDB:User"];
			string pw = configuration["LogisticsDB:Password"];
			string timeout = configuration["LogisticsDB:Timeout"];
			
			string constr =
				$"Server={server};" +
				$"Port={port};" +
				$"Database={db};" +
				$"Userid={user};" +
				$"Password={pw};" +
				$"SslMode=Require;" +
				$"Timeout={timeout};";
			// $"Protocol=3;" +
			// $"SSL=true;" +

			return new NpgsqlConnection(constr);
		}

        #endregion

    }
}
