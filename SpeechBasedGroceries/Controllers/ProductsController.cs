using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.Fridgy;

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
			List<Product> products;

			if (string.IsNullOrWhiteSpace(query))
			{
				products = fridgyClient.GetProducts();
			}
			else
			{
				products = fridgyClient.GetProductsByName(query);
			}

			return products.ToArray();
		}

		[HttpGet("{id}")]
		public Product GetById(string id)
		{
			_logger.LogInformation("Get(ById) called... Argument: id = " + id);

			Product product = new Product();
			product.Name = "PLACEHOLDER";

			return product;
		}

	}
}