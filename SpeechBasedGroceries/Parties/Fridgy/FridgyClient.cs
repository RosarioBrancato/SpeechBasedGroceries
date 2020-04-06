using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using RestSharp;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.Fridgy
{
	public class FridgyClient
	{

		private const string URL_PRODUCTS = "https://fridgy-api.herokuapp.com/api/products";

		private readonly ILogger<FridgyClient> _logger;

		private string token;

		public FridgyClient(string token)
		{
			_logger = AppLoggerFactory.GetLogger<FridgyClient>();

			this.token = token;
		}

		public List<Product> GetProducts()
		{
			RestClient client = new RestClient(URL_PRODUCTS);

			RestRequest request = new RestRequest(Method.GET);
			AddHeaderAuth(request);

			IRestResponse response = client.Execute(request);

			string content = GetResponseContent(response, "FridgyClient.GetProducts");
			List<Product> products = ConvertContentToProducts(content);

			return products;
		}

		public List<Product> GetProductsByName(string name)
		{
			RestClient client = new RestClient(URL_PRODUCTS);

			RestRequest request = new RestRequest(Method.GET);
			AddHeaderAuth(request);
			request.AddParameter("query", name);

			IRestResponse response = client.Execute(request);

			string content = GetResponseContent(response, "FridgyClient.GetProductByName");
			List<Product> products = ConvertContentToProducts(content);

			return products;
		}

		private void AddHeaderAuth(RestRequest request)
		{
			request.AddHeader("Authorization", "Bearer " + this.token);
		}


		private string GetResponseContent(IRestResponse response, string idForLogger)
		{
			string content = string.Empty;

			if (response.StatusCode == System.Net.HttpStatusCode.OK)
			{
				content = response.Content;
			}
			else
			{
				_logger.LogError(idForLogger + " - StatusCode not OK. Status description: " + response.StatusDescription);
			}

			if (response.ErrorException != null)
			{
				_logger.LogError(response.ErrorException, response.ErrorMessage);
			}

			return content;
		}


		private List<Product> ConvertContentToProducts(string content)
		{
			List<Product> products = new List<Product>();

			JArray array = JArray.Parse(content);

			foreach (var entry in array)
			{
				string productName = entry.Value<string>("name");

				Product product = new Product();
				product.Name = productName;

				products.Add(product);
			}

			return products;
		}

	}
}
