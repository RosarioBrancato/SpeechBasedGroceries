using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.Dialogflow.RequestHandler
{
	public abstract class DialogflowRequestHandler
	{

		protected WebhookRequest Request { get; private set; }

		protected WebhookResponse Response { get; private set; }


		public DialogflowRequestHandler(WebhookRequest request, WebhookResponse response)
		{
			this.Request = request;
			this.Response = response;
		}

		public static DialogflowRequestHandler Create(WebhookRequest request, WebhookResponse response)
		{
			string intentName = request.QueryResult.Intent.DisplayName;

			DialogflowRequestHandler requestHandler;
			if (intentName == DialogflowIntent.GETFRIDGEINVENTORY)
			{
				requestHandler = new GetFridgeInventoryHandler(request, response);
			}
			else
			{
				requestHandler = new DefaultHandler(request, response);
			}

			return requestHandler;
		}

		public abstract void Handle();

	}
}
