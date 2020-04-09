using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
//using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;

namespace SpeechBasedGroceries.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ProductsController : ControllerBase
	{

		private readonly ILogger<ProductsController> _logger;

		public ProductsController(ILogger<ProductsController> logger)
		{
			_logger = logger;
		}

		[HttpGet()]
		public IEnumerable<Product> GetByQuery([FromQuery(Name = "query")]string query = "")
		{
			_logger.LogInformation("GetByQuery called... Argument: query = " + query);

			//TO-DO: load user token from CRM maybe?
			string token = AppSettings.Instance.FridgyToken;

			FridgyClient fridgyClient = new FridgyClient(token);

			IList<Product> SwaggerProducts;
			if (string.IsNullOrWhiteSpace(query))
			{
				SwaggerProducts = fridgyClient.GetProducts();
			}
			else
			{
				SwaggerProducts = fridgyClient.GetProductsByName(query); 
			}

			return SwaggerProducts.ToArray<Product>();
		}

		[HttpGet("{barcode}")]
		public Product GetById(string barcode)
		{
			_logger.LogInformation("Get(ById) called... Argument: id = " + barcode);

			string token = AppSettings.Instance.FridgyToken;
			FridgyClient fridgyClient = new FridgyClient(token);
			Product product = fridgyClient.GetProductsByBarcode(barcode);
			
			return product;
		}

	}
}