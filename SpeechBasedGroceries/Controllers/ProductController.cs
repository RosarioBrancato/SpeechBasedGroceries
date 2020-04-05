using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.DTOs;

namespace SpeechBasedGroceries.Controllers
{
	/// <summary>
	/// ROUTING
	/// Example: "endpoint"/api/product/searchbyname/Milch
	/// As C# code: "endpoint"/api(defined fixed)/"class name (without the controller part)"/"method"/"method arg."/"method arg."/etc.
	/// </summary>
	[Route("api/[controller]")]
	[Authorize]
	[ApiController]
	public class ProductController : ControllerBase
	{

		private readonly ILogger<ProductController> _logger;

		public ProductController(ILogger<ProductController> logger)
		{
			_logger = logger;
		}


		[HttpGet("{id}")]
		public Product Get(string id)
		{
			Product product = new Product();
			product.Name = "GetCall";

			return product;
		}

	}
}