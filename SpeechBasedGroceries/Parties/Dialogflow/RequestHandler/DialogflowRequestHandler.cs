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


		public abstract void Handle();

	}
}
