using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace SpeechBasedGroceries.Parties.CRM
{
	public class CrmClient
	{

		private CrmDAO crmDao = new CrmDAO();
		private readonly ILogger<CrmClient> _logger;


		public CrmClient()
		{
			_logger = AppLoggerFactory.GetLogger<CrmClient>();
		}


		public List<Customer> GetCustomers()
        {
			return crmDao.GetCustomers();
		}

		public Customer GetCustomerByClientNo(string clientNo)
		{
			int cNo;
            try
            {
				cNo = Int32.Parse(clientNo);
			}
            catch(Exception e)
            {
				_logger.LogError($"clientNo must be numeric (received: {clientNo}", e);
				return null;
            }

			Customer customer = crmDao.GetCustomerByClientNo(cNo);
            if (customer == null)
            {
				_logger.LogInformation($"clientNo {cNo} does not exist");
			}

			return customer;
		}






	}
}
