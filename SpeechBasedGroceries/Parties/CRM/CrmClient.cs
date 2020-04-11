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

		public Customer GetCustomerByNo(string no)
		{
			Customer customer = null;
            if (IsValidNo(no))
            {
				customer = crmDao.GetCustomerByNo(Int32.Parse(no));
				if (customer == null)
				{
					_logger.LogInformation($"customerNo {no} does not exist");
				}
			}

			return customer;
		}


		public bool IsValidNo(string no)
        {
			bool isValid = true; // assumption
            int _no;

			try
			{
				_no = Int32.Parse(no);
			}
			catch (Exception e)
			{
				isValid = false;
				_logger.LogError(e, $"customer number «{no}» is invalid (must be numeric)");
			}

			return isValid;
        }



	}
}
