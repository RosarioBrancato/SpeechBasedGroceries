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
	public class CustomersController : ControllerBase
	{

		private readonly ILogger<CustomersController> _logger;

		public CustomersController(ILogger<CustomersController> logger)
		{
			_logger = logger;
		}

		[HttpGet()]
		public IEnumerable<Customer> GetAll()
		{
            // this function is just for testing purposes
            // (does not represent a real use case)

			_logger.LogInformation("GetAll called...");
			CrmClient crmClient = new CrmClient();
			List<Customer> customers = crmClient.GetCustomers();

			return customers.ToArray();
		}

		[HttpGet("{customerNo}")]
		public Customer GetByNo(string customerNo)
		{
			_logger.LogInformation("GetByNo called... Argument: customerNo = " + customerNo);

			CrmClient crmClient = new CrmClient();
			Customer customer = crmClient.GetCustomerByNo(customerNo);
			
			return customer;
		}

		[HttpGet("{customerNo}/deliveries")]
		public IEnumerable<Delivery> GetDeliveries(string customerNo)
		{
            // TODO: should I get the Customer object first, and then use it to query the Delivery objects?

			LogisticsClient logisticsClient = new LogisticsClient();
			List<Delivery> deliveries = logisticsClient.GetDeliveriesByCustomerNo(customerNo);

			return deliveries;
		}

		[HttpGet("{customerNo}/deliveries/{deliveryId}")]
		public Delivery GetDeliveries(string customerNo, string deliveryId)
		{
			LogisticsClient logisticsClient = new LogisticsClient();
			Delivery delivery = logisticsClient.GetDeliveryByCustomerNo(customerNo, deliveryId);
			
			return delivery;
		}

	}
}