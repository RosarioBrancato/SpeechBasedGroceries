using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Google.Cloud.Dialogflow.V2;
using Google.Protobuf;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.Parties.Dialogflow;
using SpeechBasedGroceries.Parties.Dialogflow.RequestHandler;
using SpeechBasedGroceries.Parties.Fridgy.Client.Models;

namespace SpeechBasedGroceries.Controllers
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class DialogflowController : ControllerBase
	{


		private readonly JsonParser jsonParser;
		private readonly ILogger<DialogflowController> logger;


		public DialogflowController(ILogger<DialogflowController> logger)
		{
			this.logger = logger;
			this.jsonParser = new JsonParser(JsonParser.Settings.Default.WithIgnoreUnknownFields(true));
		}


		[HttpPost]
		public async Task<ContentResult> DialogAction()
		{
			string requestJson;
			using (TextReader reader = new StreamReader(Request.Body))
			{
				requestJson = await reader.ReadToEndAsync();
			}

			WebhookRequest request = jsonParser.Parse<WebhookRequest>(requestJson);
			WebhookResponse response = new WebhookResponse();

			DialogflowRequestHandler.Create(request, response).Handle();

			string responseJson = response.ToString();

			return Content(responseJson, "application / json");
		}

	}

}