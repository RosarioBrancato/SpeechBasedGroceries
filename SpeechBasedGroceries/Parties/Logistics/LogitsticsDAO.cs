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

		[Obsolete("not a use case")]
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
							}
						}
					}
				}
			}

			return deliveries;
		}

		

		private string GetSelectAllDeliveriesStatement()
		{
			return "SELECT d.f_del_id, d.f_del_customerid, d.f_del_date, "
				+ "       d.f_del_street, d.f_del_zip, d.f_del_city, d.f_del_country, "
				+ "       d.f_del_comment "
				+ "FROM t_delivery as d ";

		}



		public Delivery UpdateDelivery(Delivery delivery)
		{
			string sql = "UPDATE [dbo].[t_delivery] "
					   + "SET f_del_customerid = (@p1), "
					   + "    f_del_date = (@p2), "
					   + "    f_del_street = (@p3), "
					   + "    f_del_zip = (@p4), "
					   + "    f_del_city = (@p5), "
					   + "    f_del_country = (@p6), "
					   + "    f_del_comment = (@p7) "
					   + "WHERE f_del_id = (@p0)";

			Delivery _delivery = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", delivery.CustomerId);
					cmd.Parameters.AddWithValue("@p2", delivery.Date);
					cmd.Parameters.AddWithValue("@p3", delivery.Street);
					cmd.Parameters.AddWithValue("@p4", delivery.Zip);
					cmd.Parameters.AddWithValue("@p5", delivery.City);
					cmd.Parameters.AddWithValue("@p6", delivery.Country);
					cmd.Parameters.AddWithValue("@p7", delivery.Comment);
					cmd.Parameters.AddWithValue("@p0", delivery.Id);

					if (cmd.ExecuteNonQuery() == 1)
					{
						_delivery = delivery;
					};
				}
			}

			return _delivery;
		}

		public Delivery CreateDelievery(Delivery delivery)
		{
			string sql = "INSERT INTO [dbo].[t_delivery] "
					   + "  (f_del_customerid, f_del_date, "
					   + "   f_del_street, f_del_zip, f_del_city, f_del_country, "
					   + "   f_del_comment) "
					   + "VALUES "
					   + "  ((@p1), (@p2), "
					   + "   (@p3), (@p4), (@p5), (@p6), "
					   + "   (@p7)) "
					   + "SELECT SCOPE_IDENTITY()";

			Delivery _delivery = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", delivery.CustomerId);
					cmd.Parameters.AddWithValue("@p2", delivery.Date);
					cmd.Parameters.AddWithValue("@p3", delivery.Street);
					cmd.Parameters.AddWithValue("@p4", delivery.Zip);
					cmd.Parameters.AddWithValue("@p5", delivery.City);
					cmd.Parameters.AddWithValue("@p6", delivery.Country);
					cmd.Parameters.AddWithValue("@p7", delivery.Comment);

					int deliveryId = 0;
					try
					{
						deliveryId = Int32.Parse(cmd.ExecuteScalar().ToString());
						delivery.Id = deliveryId;
						_delivery = delivery;
					}
					catch (Exception e)
					{
						Console.Write(e.StackTrace);
					}
				}
			}

			return _delivery;
		}


		#endregion




		#region position queries


		private List<Position> GetPositions(int deliveryId)
        {
            string sql =
				GetSelectAllPositionsStatement()
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


		public Position GetPositionById(int positionId)
		{
			string sql =
				  GetSelectAllPositionsStatement()
				+ "WHERE p.f_pos_id = (@p1)";

			Position position = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", positionId);
					using (SqlDataReader reader = cmd.ExecuteReader())
					{
						if (reader != null)
						{
							while (reader.Read())
							{
								//should only have 1 record
								position = MappingPosition(reader);
							}
						}
					}
				}
			}

			return position;
		}


		private string GetSelectAllPositionsStatement()
		{
			return "SELECT p.f_pos_id, p.f_del_id, p.f_pos_no, "
				+ "       p.f_pos_itemid, p.f_pos_itemtext, p.f_pos_itemqty, p.f_pos_itemprice, p.f_pos_itemweight, "
				+ "       p.f_pos_comment "
				+ "FROM t_position as p ";
		}


		public Position UpdatePosition(Position position)
		{
			string sql = "UPDATE [dbo].[t_position] "
					   + "SET f_pos_no = (@p1), "
					   + "    f_pos_itemid = (@p2), "
					   + "    f_pos_itemtext = (@p3), "
					   + "    f_pos_itemqty = (@p4), "
					   + "    f_pos_itemprice = (@p5), "
					   + "    f_pos_itemweight = (@p6), "
					   + "    f_pos_comment = (@p7) "
					   + "WHERE f_pos_id = (@p8)"
                       + "  AND f_del_id = (@p9)";

			Position _position = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", position.No);
					cmd.Parameters.AddWithValue("@p2", position.ItemId);
					cmd.Parameters.AddWithValue("@p3", position.ItemText);
					cmd.Parameters.AddWithValue("@p4", position.ItemQty);
					cmd.Parameters.AddWithValue("@p5", position.ItemPrice);
					cmd.Parameters.AddWithValue("@p6", position.ItemWeight);
					cmd.Parameters.AddWithValue("@p7", position.Comment);
					cmd.Parameters.AddWithValue("@p8", position.Id);
					cmd.Parameters.AddWithValue("@p9", position.DeliveryId);

					if (cmd.ExecuteNonQuery() == 1)
					{
						_position = position;
					};
				}
			}

			return _position;
		}


		public Position CreatePosition(Position position)
		{
			string sql = "INSERT INTO [dbo].[t_position] "
					   + "  (f_del_id, f_pos_no, "
					   + "   f_pos_itemid, f_pos_itemtext, "
                       + "   f_pos_itemqty, f_pos_itemprice, f_pos_itemweight, "
					   + "   f_pos_comment) "
					   + "VALUES "
					   + "  ((@p1), (@p2), "
					   + "   (@p3), (@p4), "
                       + "   (@p5), (@p6), (@p7), "
					   + "   (@p8)) "
					   + "SELECT SCOPE_IDENTITY()";

			Position _position = null;

			using (SqlConnection connection = this.getConnection())
			{
				connection.Open();
				using (SqlCommand cmd = new SqlCommand(sql, connection))
				{
					cmd.Parameters.AddWithValue("@p1", position.DeliveryId);
					cmd.Parameters.AddWithValue("@p2", position.No);
					cmd.Parameters.AddWithValue("@p3", position.ItemId);
					cmd.Parameters.AddWithValue("@p4", position.ItemText);
					cmd.Parameters.AddWithValue("@p5", position.ItemQty);
					cmd.Parameters.AddWithValue("@p6", position.ItemPrice);
					cmd.Parameters.AddWithValue("@p7", position.ItemWeight);
					cmd.Parameters.AddWithValue("@p8", position.Comment);

					int positionId = 0;
					try
					{
						positionId = Int32.Parse(cmd.ExecuteScalar().ToString());
						position.Id = positionId;
						_position = position;
					}
					catch (Exception e)
					{
						Console.Write(e.StackTrace);
					}
				}
			}

			return _position;
		}



		#endregion







		#region mapping profiles

		private Delivery MappingDelivery(SqlDataReader dr)
        {
            Delivery delivery = new Delivery()
            {
                Id = Int32.Parse(dr["f_del_id"].ToString()),
                CustomerId = dr.IsDBNull("f_del_customerid") ? default : Int32.Parse(dr["f_del_customerid"].ToString()),
                Date = dr.IsDBNull("f_del_date") ? default : DateTime.Parse(dr["f_del_date"].ToString()),
                Street = dr.IsDBNull("f_del_street") ? default : dr["f_del_street"].ToString(),
                Zip = dr.IsDBNull("f_del_zip") ? default : dr["f_del_zip"].ToString(),
                City = dr.IsDBNull("f_del_city") ? default : dr["f_del_city"].ToString(),
                Country = dr.IsDBNull("f_del_country") ? default : dr["f_del_country"].ToString(),
                Comment = dr.IsDBNull("f_del_comment") ? default : dr["f_del_comment"].ToString(),
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
				No = dr.IsDBNull("f_pos_no") ? default : Int32.Parse(dr["f_pos_no"].ToString()),
                ItemId = dr.IsDBNull("f_pos_itemid") ? default : dr["f_pos_itemid"].ToString(),
                ItemText = dr.IsDBNull("f_pos_itemtext") ? default : dr["f_pos_itemtext"].ToString(),
                ItemQty = dr.IsDBNull("f_pos_itemqty") ? default : Int32.Parse(dr["f_pos_itemqty"].ToString()),
                ItemPrice = dr.IsDBNull("f_pos_itemprice") ? default : Double.Parse(dr["f_pos_itemprice"].ToString()),
                ItemWeight = dr.IsDBNull("f_pos_itemweight") ? default : Double.Parse(dr["f_pos_itemweight"].ToString()),
                Comment = dr.IsDBNull("f_pos_comment") ? default : dr["f_pos_comment"].ToString()
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
