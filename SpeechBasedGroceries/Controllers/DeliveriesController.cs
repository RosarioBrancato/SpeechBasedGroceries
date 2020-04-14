using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Logistics;
namespace SpeechBasedGroceries.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class DeliveriesController : ControllerBase
	{
        
		private readonly ILogger<DeliveriesController> _logger;

		public DeliveriesController(ILogger<DeliveriesController> logger)
		{
			_logger = logger;
		}

		[Obsolete("Deliveries should be queries api/{customerNo}/deliveries/{deliveryId}")]
		[HttpGet()]
		public IEnumerable<Delivery> GetAll()
		{
			// this function is just for testing purposes
			// (does not represent a real use case)

			LogisticsClient logisticsClient = new LogisticsClient();
			List<Delivery> deliveries = logisticsClient.GetDeliveries();
			
			return deliveries.ToArray();
		}

		/* // NO LONGER IN USE
         * 
		[Obsolete("Deliveries should be queries api/{customerId}/deliveries/{deliveryId}")]
		[HttpGet("{deliveryId}")]
		public Delivery GetById(string deliveryId)
		{
			_logger.LogInformation("GetById called... Argument: deliveryId = " + deliveryId);

			LogisticsClient logisticsClient = new LogisticsClient();
			Delivery delivery = logisticsClient.GetDeliveryById(deliveryId);

			return delivery;
		}

        
        //// ----> see GetAll()
        
		//[HttpGet()]
		//public IEnumerable<Delivery> GetByCustomerNo([FromQuery(Name = "customerNo")]string customerNo = "")
		//{
		//	_logger.LogInformation("GetByCustomerNo called... Argument: customerNo = " + customerNo);
        //
		//	LogisticsClient logisticsClient = new LogisticsClient();
		//	List<Delivery> deliveries = logisticsClient.GetDeliveriesByCustomerNo(customerNo);
        //
		//	return deliveries;
		//}
        *
        */
	}
}