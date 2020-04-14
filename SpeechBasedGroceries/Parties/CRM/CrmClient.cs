﻿using Microsoft.Extensions.Logging;
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


        // only used for unit tests
		public CrmClient(ILogger<CrmClient> logger)
		{
			_logger = logger;
		}


		#region client functions

		public List<Customer> GetCustomers()
        {
			return crmDao.GetCustomers();
		}


		public Customer GetCustomerById(string id)
		{
			Customer customer = null;
            if (IsValidId(id))
            {
				customer = crmDao.GetCustomerById(Int32.Parse(id));
				if (customer == null)
				{
					_logger.LogInformation($"customer with ID «{id}» does not exist");
				}
			}

			return customer;
		}


		public Customer GetCustomerByTelegramId(string telegramId)
		{
			Customer customer = null;
			if (IsValidTelegramId(telegramId))
			{
				customer = crmDao.GetCustomerByTelegramId(Int32.Parse(telegramId));
				if (customer == null)
				{
					_logger.LogInformation($"customer with TelegramID «{telegramId}» does not exist");
				}
			}

			return customer;
		}

		       

		public Customer CreateUpdateCustomer(Customer customer, bool includeTokens = false)
        {
			Customer _customer;
            if (crmDao.GetCustomerById(customer.Id) == null)
            {
				_customer = crmDao.CreateCustomer(customer);
				if (includeTokens)
				{
					_customer.Tokens.ForEach(tok => crmDao.CreateToken(tok));
				}
			} else
            {
				_customer = crmDao.UpdateCustomer(customer);
				if (includeTokens)
				{
					_customer.Tokens.ForEach(tok => crmDao.UpdateToken(tok));
				}
			}

			return _customer;
        }


		public Token CreateUpdateToken(Token token)
		{
			Token _token;
			if (crmDao.GetTokenById(token.Id) == null)
			{
				_token = crmDao.CreateToken(token);

			}
			else
			{
				_token = crmDao.UpdateToken(token);
			}

			return _token;
		}

		#endregion


		#region validation

		public bool IsValidId(string id)
        {
			bool isValid = true; // assumption
            int _id;

			try
			{
				_id = Int32.Parse(id);
			}
			catch (Exception e)
			{
				isValid = false;
				_logger.LogError(e, $"customer ID «{id}» is invalid (must be numeric)");
			}

			return isValid;
        }

		public bool IsValidTelegramId(string id)
		{
			bool isValid = true; // assumption
			int _id;

			try
			{
				_id = Int32.Parse(id);
			}
			catch (Exception e)
			{
				isValid = false;
				_logger.LogError(e, $"customer TelegramId «{id}» is invalid (must be numeric)");
			}

			return isValid;
		}

		#endregion

	}
}
