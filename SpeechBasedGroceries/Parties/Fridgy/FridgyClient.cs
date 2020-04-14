
using f = SpeechBasedGroceries.Parties.Fridgy.Client;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeechBasedGroceries.Parties.Fridgy.Client;

namespace SpeechBasedGroceries.Parties.Fridgy
{
	public class FridgyClient
	{
		private string token;

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
		public string Token
		{
			get { return token; }
			set
			{
				token = value;
				credentials = new TokenCredentials(value);
				client = new f.Fridgy(credentials);

			}
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

	}
}
