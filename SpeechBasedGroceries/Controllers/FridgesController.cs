using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;

namespace SpeechBasedGroceries.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class FridgesController : ControllerBase
	{

		private readonly ILogger<FridgesController> _logger;

		public FridgesController(ILogger<FridgesController> logger)
		{
			_logger = logger;
		}

		[HttpGet()]
		public IEnumerable<Fridge> Get()
		{
			_logger.LogInformation("Get Fridges");

			//TO-DO: load user token from CRM maybe?
			string token = AppSettings.Instance.FridgyToken;

			FridgyClient fridgyClient = new FridgyClient(token);

			IList<Fridge> SwaggerFridges;

			SwaggerFridges = fridgyClient.GetFridges(); 
			

			return SwaggerFridges.ToArray<Fridge>();
		}

	}
}