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
		public IEnumerable<Product> Get()
		{
			List<Product> products = new List<Product>();
			products.Add(new Product() { Name = "All the products" });

			return products.ToArray();
		}

		[HttpGet("{name}")]
		public IEnumerable<Product> Get(string name)
		{
			_logger.LogInformation("Searching by name... Argument: " + name);

			//TO-DO: load user token from CRM maybe?
			string token = AppSettings.Instance.FridgyToken;

			FridgyClient fridgyClient = new FridgyClient(token);
			List<Product> products = fridgyClient.GetProductsByName(name);

			return products.ToArray();
		}

	}
}