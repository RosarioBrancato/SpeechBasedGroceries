
using f = SpeechBasedGroceries.Parties.Fridgy.Client;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpeechBasedGroceries.AppServices;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeechBasedGroceries.Parties.Fridgy.Client;
using Microsoft.Rest;
using SpeechBasedGroceries.DTOs;
using Microsoft.VisualBasic.CompilerServices;

namespace SpeechBasedGroceries.Parties.Fridgy
{
	public class FridgyClient
	{

		private readonly ILogger<FridgyClient> _logger;

		private Microsoft.Rest.ServiceClientCredentials credentials;

		private f.Fridgy client;

		// TODO: overload constructor with FridgyClient() (to be used for account creation)
		public FridgyClient()
		{
			_logger = AppLoggerFactory.GetLogger<FridgyClient>();
			// TODO: better way than give this dummy string to the credentials constructor
			client = new f.Fridgy(new TokenCredentials("empty"));
		}

		public void setToken(string value)
		{
			credentials = new TokenCredentials(value);
			client = new f.Fridgy(credentials);

		}
		public void setBasicAuth(string username, string password) {
			BasicAuthenticationCredentials credentials = new BasicAuthenticationCredentials();
			credentials.Password = password;
			credentials.UserName = username;
			client = new f.Fridgy(credentials);
		}

		public IList<Product> GetProducts()
		{
			IList<Product> productlist = client.Get.Products();
			return productlist;
		}

		public IList<Product> GetProductsByName(string name)
		{
			IList<Product> productlist = client.Get.Products("name.asc", name);
			return productlist;
		}
		public Product GetProductsByBarcode(string barcode)
		{
			Product product = client.Get.Barcode(barcode);
			return product;
		}

		public IList<Fridge> GetFridges()
		{
			IList<Fridge> fridges = client.Get.Fridges();
			return fridges;
		}

		public IList<Item> GetItems(string fridgeUUID) {
			IList<Item> items = client.Get.Items(fridgeUUID);
			return items;
		}

		public User RegisterUser(string username, string password, string displayname, string email) {
			PostUser user = new PostUser();
			user.Displayname = displayname;
			user.Username = username;
			user.Password = password;
			user.Email = email;
			User createdUser = client.Create.UserMethod(user);
			return createdUser;
		}

		public Fridge CreateNewFridge(string name) {
			BaseFridge fridge = new BaseFridge();
			fridge.Name = name;
			return client.Create.Fridges(fridge);
		}

		public Fridge AddUserToFridge(string userUUID, string fridgeUUID) {
			Owner owner = new Owner();
			owner.Uuid = Guid.Parse(userUUID);
			return client.Add.Owners(owner, fridgeUUID);
		}

		public void RemoveUserFromFridge(string userUUID, string fridgeUUID)
		{
			client.Delete.Owners(fridgeUUID, userUUID);
		}

		public void DeleteUser(string uuid)
		{
			client.Delete.Users(uuid);
		}

		public string RetrieveToken() {
			TokensResponse resp = client.Get.Jwttoken();
			if (!(resp is null)) return resp.Token;
			return null;
		}

	}
}
