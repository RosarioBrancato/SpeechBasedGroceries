using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using System;
using SpeechBasedGroceries.AppServices;
using System.IdentityModel.Tokens.Jwt;

namespace SpeechBasedGroceries.BusinessLogic
{
	public class Registrar
	{

		private readonly ILogger<Registrar> logger;
		private CrmClient crmClient;

		public Registrar()
		{
			this.logger = AppLoggerFactory.GetLogger<Registrar>();
			this.crmClient = new CrmClient();
		}

		public Customer RegisterTelegramUser(TelegramUser telegramUser)
		{
			Customer customer = this.crmClient.GetCustomerByTelegramId(telegramUser.Id);

			if (customer == null)
			{
				//fridgy placeholders?
				string uuid = Guid.NewGuid().ToString();
				var username = "TelegramUser-" + uuid.Substring(0, 4);
				var password = uuid;
				var email = username + "@google.com";
				var displayname = username;

				string bearerToken = this.CreateNewFridgyAccount(username, password, displayname, email);
				JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
				JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(bearerToken);

				//create customer
				customer = new Customer();
				customer.Firstname = telegramUser.FirstName;
				customer.Surname = telegramUser.LastName;
				customer.TelegramId = telegramUser.Id;
				customer.Email = email;

				customer = this.crmClient.CreateUpdateCustomer(customer);

				//add token to customer
				Token token = new Token();
				token.CustomerId = customer.Id;
				token.Name = "Fridgy";
				token.Value = bearerToken;
				token.Creation = jwtToken.ValidFrom;
				token.Expiration = jwtToken.ValidTo;

				this.crmClient.CreateUpdateToken(token);

				//get a fresh customer with token
				customer = this.crmClient.GetCustomerById(customer.Id);
			}

			return customer;
		}


		/// <summary>
		/// Creates new Fridgy account.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="displayname"></param>
		/// <param name="email"></param>
		/// <returns>Bearer token of newly created account</returns>
		private string CreateNewFridgyAccount(string username, string password, string displayname, string email)
		{
			FridgyClient fridgyClient = new FridgyClient();
			User newUser = fridgyClient.RegisterUser(username, password, displayname, email);
			fridgyClient.SetBasicAuth(username, password);
			string token = fridgyClient.RetrieveToken();
			fridgyClient.SetToken(token);
			Fridge newfridge = fridgyClient.CreateNewFridge(username + "-fridge");

			this.logger.LogInformation("new fridge " + newfridge.Id.ToString());

			// here we add the admin user to every fridge we create (for debugging purposes
			// todo: remove in production
			// TODO: do we really need this since we now creat new individual accounts?
			fridgyClient.AddUserToFridge("1c5c1da9-27e1-419f-872b-90d89854ce5d", newfridge.Id.ToString());
			return token;
		}

	}
}
