using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.BusinessLogic
{
	public class Registrar
	{

		public bool LoginWithTelegram(string telegramId)
		{
			//TO-DO
			//CRM: check if telegramId exists in t_customer
			//CRM: if not -> customer = Registrar.RegisterCustomerInCrm(...)
			//            -> token_string = Registrar.CreateNewFridgyAccount(customer.Id)
			//            -> Registrar.AssignTokenToCustomer(customer, token_name, token_value, token_expiration)
			return false;
		}


        /*
         * Purpose: Creates new Fridgy account.
         * Params:  none
         * Return:  Bearer token of newly created account
         */
        public string CreateNewFridgyAccount(int customerId)
		{
            // TODO
            // random username and password (may be seeded with customerId)
			return null;
		}


		/*
         * Purpose: Entries customer in CRM DB.
         * Params:  self explaining
         * Return:  Customer object if succeeded, null if failed
         */
		public Customer RegisterCustomerInCrm(
            string firstname = default,
            string surname = default,
            DateTime birthdate = default(DateTime),
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
		public Token AssignTokenToCustomer(
            Customer customer,
            string token_name,
            string token_value,
            DateTime token_expiration = default)
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
