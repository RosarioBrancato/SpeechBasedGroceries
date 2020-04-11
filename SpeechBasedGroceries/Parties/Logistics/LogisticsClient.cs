using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.Logistics
{
	public class LogisticsClient
	{

		private LogisticsDAO logisticsDao = new LogisticsDAO();
		private readonly ILogger<LogisticsClient> _logger;


		public LogisticsClient()
		{
			_logger = AppLoggerFactory.GetLogger<LogisticsClient>();
		}


		public List<Delivery> GetDeliveries()
		{
			return logisticsDao.GetDeliveries();
		}

        public List<Delivery> GetDeliveries(Customer customer)
        {
            return logisticsDao.GetDeliveriesByCustomerNo(customer.No);
        }

        public List<Delivery> GetDeliveriesByQuery(string customerNo = null, string date = null)
        {


			// initiate parameter variables
			int? _customerNo = null;
			DateTime? _date = null;

            // validate parameters
			bool validQuery = true;
            if (validQuery && !String.IsNullOrEmpty(customerNo))
            {
				if (IsValidCustomerNo(customerNo)) {
                    _customerNo = Int32.Parse(customerNo);
				} else
                {
					validQuery = false;
                }
            }
			if (validQuery && !String.IsNullOrEmpty(date))
			{
				if (IsValidDate(date))
				{
					_date = DateTime.Parse(date);
				}
				else
				{
					validQuery = false;
				}
			}


			//perform query
			List<Delivery> deliveries = new List<Delivery>();
			if (validQuery)
            {
				deliveries = logisticsDao.GetDeliveriesByQuery(_customerNo, _date);
            } else
            {
				_logger.LogWarning($"cannot understand query");
			}

			return deliveries;
        }

		public Delivery GetDeliveryById(string Id)
		{
			Delivery delivery = null;
            if (IsValidDeliveryId(Id))
            {
				delivery = logisticsDao.GetDeliveryById(Int32.Parse(Id));
			}
            return delivery;
		}

		public List<Delivery> GetDeliveriesByCustomerNo(string customerNo)
		{
			List<Delivery> deliveries = new List<Delivery>();
			if (IsValidCustomerNo(customerNo))
			{
				deliveries = logisticsDao.GetDeliveriesByCustomerNo(Int32.Parse(customerNo));
			}

			return deliveries;
		}

        public Delivery GetDeliveryByCustomerNo(string customerNo, string deliveryId)
        {
            Delivery delivery = null;
            if (IsValidCustomerNo(customerNo) && IsValidDeliveryId(deliveryId))
            {
                delivery = logisticsDao.GetDeliveryByCustomerNo(Int32.Parse(customerNo), Int32.Parse(deliveryId));
            }

            return delivery;
        }



        #region validation

        public bool IsValidCustomerNo(string customerNo)
        {
            bool isValid = true; // assumption
            int _customerNo;

            try
            {
                _customerNo = Int32.Parse(customerNo);
            }
            catch (Exception e)
            {
                isValid = false;
                _logger.LogError(e, $"customer number «{customerNo}» is invalid (must be numeric)");
            }

            return isValid;
        }


        public bool IsValidDeliveryId(string deliveryId)
        {
            bool isValid = true; // assumption
            int _deliveryId;

            try
            {
                _deliveryId = Int32.Parse(deliveryId);
            }
            catch (Exception e)
            {
                isValid = false;
                _logger.LogError(e, $"delivery ID «{deliveryId}» is invalid (must be numeric)");
            }

            return isValid;
        }

        public bool IsValidDate(string date)
        {
            bool isValid = true; // assumption
            DateTime _date;

            try
            {
                _date = DateTime.Parse(date);
            }
            catch (Exception e)
            {
                isValid = false;
                _logger.LogError(e, $"delivery date «{date}» could not be parsed");
            }

            return isValid;
        }

        #endregion
    }
}
