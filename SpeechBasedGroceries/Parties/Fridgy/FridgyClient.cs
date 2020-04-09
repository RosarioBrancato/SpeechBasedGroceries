
using f = SpeechBasedGroceries.Parties.Fridgy.Client;
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

		private readonly ILogger<FridgyClient> _logger;

		private Microsoft.Rest.ServiceClientCredentials credentials;

		private f.Fridgy client;

		public FridgyClient(string token)
		{
			_logger = AppLoggerFactory.GetLogger<FridgyClient>();
			credentials = new TokenCredentials(token);
			client = new f.Fridgy(credentials);
		}


		public IList<f.Models.Product> GetProducts()
		{
			IList<f.Models.Product> prods = client.Get.Products();
			return prods;
		}

		public IList<f.Models.Product> GetProductsByName(string name)
		{

			IList<f.Models.Product> prods = client.Get.Products("name.asc", name);
			return prods;
		}
		public f.Models.Product GetProductsByBarcode(string barcode)
		{

			f.Models.Product prod = client.Get.Barcode(barcode);
			return prod;
		}

		public IList<f.Models.Fridge> GetFridges()
		{
			IList<f.Models.Fridge> fr = client.Get.Fridges();
			return fr;
		}


	}
}
