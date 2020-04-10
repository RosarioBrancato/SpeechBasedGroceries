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
		public IEnumerable<Customer> GetCustomers()
		{
            // this function is just for testing purposes
            // (does not represent a real use case)

			_logger.LogInformation("GetCustomers called...");
			CrmClient crmClient = new CrmClient();
			List<Customer> customers = crmClient.GetCustomers();

			return customers.ToArray();
		}

		[HttpGet("{clientNo}")]
		public Customer GetByClientNo(string clientNo)
		{
			_logger.LogInformation("GetByClientNo called... Argument: clientNo = " + clientNo);

			CrmClient crmClient = new CrmClient();
			Customer customer = crmClient.GetCustomerByClientNo(clientNo);
			
			return customer;
		}

	}
}