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

		private CrmDAO crmDao;
		private readonly ILogger<CrmClient> logger;


		public CrmClient()
		{
			this.logger = AppLoggerFactory.GetLogger<CrmClient>();
			this.crmDao = new CrmDAO();
		}


		#region client functions

		public List<Customer> GetCustomers()
		{
			return this.crmDao.GetCustomers();
		}


		public Customer GetCustomerById(string id)
		{
			Customer customer = null;
			if (IsValidId(id))
			{
				customer = crmDao.GetCustomerById(int.Parse(id));
				if (customer == null)
				{
					this.logger.LogInformation($"customer with ID «{id}» does not exist");
				}
			}

			return customer;
		}

		public Customer GetCustomerById(int id)
		{
			return GetCustomerById(id.ToString());
		}

		public Customer GetCustomerByTelegramId(int telegramId)
		{
			Customer customer = null;
			if (IsValidTelegramId(telegramId))
			{
				customer = crmDao.GetCustomerByTelegramId(telegramId);
				if (customer == null)
				{
					this.logger.LogInformation($"customer with TelegramID «{telegramId}» does not exist");
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
				if (includeTokens && _customer != null)
				{
					_customer.Tokens.ForEach(t => t.CustomerId = _customer.Id);
					_customer.Tokens.ForEach(t => crmDao.CreateToken(t));
				}
			}
			else
			{
				_customer = crmDao.UpdateCustomer(customer);
				if (includeTokens)
				{
					_customer.Tokens.ForEach(tok => crmDao.UpdateToken(tok));
				}
			}

			return _customer;
		}


		public bool DeleteCustomer(Customer customer)
		{
			bool success = false;
			if (crmDao.GetCustomerById(customer.Id) != null)
			{
				success = crmDao.DeleteCustomer(customer);
			}
			else
			{
				this.logger.LogInformation($"customer with ID «{customer.Id}» does not exist");
			}

			return success;
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


		public bool DeleteToken(Token token)
		{
			bool success = false;
			if (crmDao.GetTokenById(token.Id) != null)
			{
				success = crmDao.DeleteToken(token);
			}
			else
			{
				this.logger.LogInformation($"token with ID «{token.Id}» does not exist");
			}

			return success;
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
				this.logger.LogError(e, $"customer ID «{id}» is invalid (must be numeric)");
			}

			return isValid;
		}

		public bool IsValidTelegramId(int id)
		{
			bool isValid = true; // assumption

			if (id <= 0)
			{
				isValid = false;
				this.logger.LogError($"customer TelegramId «{id}» is invalid (must be numeric)");
			}

			return isValid;
		}

		#endregion

	}
}
