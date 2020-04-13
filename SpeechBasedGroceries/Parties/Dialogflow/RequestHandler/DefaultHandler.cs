using Google.Cloud.Dialogflow.V2;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.Dialogflow.RequestHandler
{
	public class DefaultHandler : DialogflowRequestHandler
	{

		public DefaultHandler(WebhookRequest request, WebhookResponse response) : base(request, response)
		{
		}

		public override void Handle()
		{
			this.Response.FulfillmentText = "Request with reponse id " + this.Request.ResponseId + " handled at " + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
		}

	}
}
