using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using System;
using SpeechBasedGroceries.AppServices;

namespace SpeechBasedGroceries.BusinessLogic
{
	public class Registrar
	{

		private CrmClient crmClient;

		public Registrar()
		{
		}

		public Token LoginWithTelegram(string telegramId)
		{
			this.crmClient = new CrmClient();
			Customer customer = this.crmClient.GetCustomerByTelegramId(telegramId);
			Token token;
			if (customer is null)
			{
				String UUID = Guid.NewGuid().ToString();
				var username = "lonelyuser-" + UUID.Substring(0, 4);
				var password = UUID;
				var email = username + "@google.com";
				var displayname = username;
				var intTelegramid = Int32.Parse(telegramId);

				Customer newCustomer = RegisterCustomerInCrm(null, null, null, null, null, null, null, email, intTelegramid);
				string bearerToken = CreateNewFridgyAccount(username, password, displayname, email);
				token = AssignTokenToCustomer(newCustomer, "Fridgy", bearerToken);
			}
			else
			{
				token = customer.GetFridigyToken();
			}

            // TODO: return whole Customer instead
			return token;
		}


		/*
         * Purpose: Creates new Fridgy account.
         * Params:  none
         * Return:  Bearer token of newly created account
         */
		private string CreateNewFridgyAccount(string username, string password, string displayname, string email)
		{
			FridgyClient fridgyClient = new FridgyClient();
			User newUser = fridgyClient.RegisterUser(username, password, displayname, email);
			fridgyClient.setBasicAuth(username, password);
			string token = fridgyClient.RetrieveToken();
			fridgyClient.setToken(token);
			Fridge newfridge = fridgyClient.CreateNewFridge(username + "-fridge");
			Console.WriteLine("new fridge " + newfridge.Id.ToString());

			// here we add the admin user to every fridge we create (for debugging purposes
			// todo: remove in production
			fridgyClient.AddUserToFridge("1c5c1da9-27e1-419f-872b-90d89854ce5d", newfridge.Id.ToString());
			return token;
		}


		/*
         * Purpose: Entries customer in CRM DB.
         * Params:  self explaining
         * Return:  Customer object if succeeded, null if failed
         */
		private Customer RegisterCustomerInCrm(
			string firstname = default,
			string surname = default,
			DateTime? birthdate = null,
			string street = default,
			string zip = default,
			string city = default,
			string country = default,
			string email = default,
			int telegramid = default)
		{

			Customer customer = new Customer()
			{
				Firstname = firstname,
				Surname = surname,
				Birthdate = birthdate,
				Street = street,
				Zip = zip,
				City = city,
				Country = country,
				Email = email,
				TelegramId = telegramid,
				Tokens = null
			};

			return new CrmClient().CreateUpdateCustomer(customer);
		}


		/*
         * Purpose: Assignes a token to a customer.
         * Params:  self explaining
         * Return:  Customer object if succeeded, null if failed
         */
		private Token AssignTokenToCustomer(
			Customer customer,
			string token_name,
			string token_value,
			DateTime? token_expiration = null)
		{

			Token token = new Token()
			{
				CustomerId = customer.Id,
				Creation = DateTime.Now,
				Name = token_name,
				Value = token_value,
				Expiration = token_expiration
			};

			return new CrmClient().CreateUpdateToken(token);
		}



	}
}
