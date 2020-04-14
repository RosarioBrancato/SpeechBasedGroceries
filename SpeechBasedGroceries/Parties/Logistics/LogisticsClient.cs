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

        // only used for unit tests
        public LogisticsClient(ILogger<LogisticsClient> logger)
        {
            _logger = logger;
        }



        #region client functions

        [Obsolete("not a use case")]
        public List<Delivery> GetDeliveries()
		{
			return logisticsDao.GetDeliveries();
		}

        
        [Obsolete("not a use case")]
        public List<Delivery> GetDeliveriesByQuery(string customerId = null, string date = null)
        {

			// initiate parameter variables
			int? _customerNo = null;
			DateTime? _date = null;

            // validate parameters
			bool validQuery = true;
            if (validQuery && !String.IsNullOrEmpty(customerId))
            {
				if (IsValidCustomerNo(customerId)) {
                    _customerNo = Int32.Parse(customerId);
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
                if (!deliveries.Any())
                {
                    _logger.LogInformation($"no deliveries found that match query");
                }
            } else
            {
				_logger.LogWarning($"cannot understand query");
			}

			return deliveries;
        }

		public Delivery GetDeliveryById(string deliveryId)
		{
			Delivery delivery = null;
            if (IsValidDeliveryId(deliveryId))
            {
				delivery = logisticsDao.GetDeliveryById(Int32.Parse(deliveryId));
                if (delivery == null)
                {
                    _logger.LogInformation($"deliveries with ID «{deliveryId}» does not exist");
                }
            }
            return delivery;
		}

		public List<Delivery> GetDeliveriesByCustomerId(string customerId)
		{
			List<Delivery> deliveries = new List<Delivery>();
			if (IsValidCustomerNo(customerId))
			{
				deliveries = logisticsDao.GetDeliveriesByCustomerId(Int32.Parse(customerId));
                if (!deliveries.Any())
                {
                    _logger.LogInformation($"no deliveries found for customer with ID «{customerId}»");
                }
            }

			return deliveries;
		}


        public Delivery CreateUpdateDelivery(Delivery delivery, bool includePositions = false)
        {
            Delivery _delivery;
            if (logisticsDao.GetDeliveryById(delivery.Id) == null)
            {
                _delivery = logisticsDao.CreateDelievery(delivery);
                if (includePositions)
                {
                    _delivery.Positions.ForEach(pos => logisticsDao.CreatePosition(pos));
                }
            }
            else
            {
                _delivery = logisticsDao.UpdateDelivery(delivery);
                if (includePositions)
                {
                    _delivery.Positions.ForEach(pos => logisticsDao.UpdatePosition(pos));
                }
            }

            return _delivery;
        }


        public Position CreateUpdatePosition(Position position)
        {
            Position _position;
            if (logisticsDao.GetPositionById(position.Id) == null)
            {
                _position = logisticsDao.CreatePosition(position);

            }
            else
            {
                _position = logisticsDao.UpdatePosition(position);
            }

            return _position;
        }


        #endregion



        #region validation

        public bool IsValidCustomerNo(string customerId)
        {
            bool isValid = true; // assumption
            int _customerNo;

            try
            {
                _customerNo = Int32.Parse(customerId);
            }
            catch (Exception e)
            {
                isValid = false;
                _logger.LogError(e, $"customer id «{customerId}» is invalid (must be numeric)");
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
                isValid = _deliveryId >= 100 ? isValid : false;
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
                //_date = DateTime.ParseExact(date, "yyyy-MM-dd", null);
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
