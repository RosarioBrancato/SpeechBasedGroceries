using Google.Cloud.Dialogflow.V2;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.Dialogflow.RequestHandler
{
	public class GetFridgeInventoryHandler : DialogflowRequestHandler
	{

		private readonly ILogger<GetFridgeInventoryHandler> logger;

		public GetFridgeInventoryHandler(WebhookRequest request, WebhookResponse response) : base(request, response)
		{
			this.logger = AppLoggerFactory.GetLogger<GetFridgeInventoryHandler>();
		}

		public override void Handle()
		{
			var inventory = this.GetData();

			if (inventory == null  || inventory?.Items.Count == 0)
			{
				this.Response.FulfillmentText = "Your fridge is empty.";
			}
			else
			{
				//general
				string responseText = "Here’s your current inventory:" + Environment.NewLine + string.Join(Environment.NewLine, inventory.Items.Select(s => string.Concat(s.Quantity, "x ", s.Name)));
				this.Response.FulfillmentText = responseText;

				Intent.Types.Message messageResponse = new Intent.Types.Message();
				messageResponse.Text = new Intent.Types.Message.Types.Text();
				messageResponse.Text.Text_.Add(responseText);

				//telegram
				Intent.Types.Message teleMessageResponse = new Intent.Types.Message();
				teleMessageResponse.Platform = Intent.Types.Message.Types.Platform.Telegram;
				teleMessageResponse.Text = new Intent.Types.Message.Types.Text();
				teleMessageResponse.Text.Text_.Add(responseText);

				Intent.Types.Message teleMessageQuickReplies = new Intent.Types.Message();
				teleMessageQuickReplies.Platform = Intent.Types.Message.Types.Platform.Telegram;
				teleMessageQuickReplies.QuickReplies = new Intent.Types.Message.Types.QuickReplies();
				teleMessageQuickReplies.QuickReplies.Title = "Do you want to order the results?";
				teleMessageQuickReplies.QuickReplies.QuickReplies_.Add("Yes, alphabetically.");
				teleMessageQuickReplies.QuickReplies.QuickReplies_.Add("Yes, by due date.");
				teleMessageQuickReplies.QuickReplies.QuickReplies_.Add("No.");

				this.Response.FulfillmentMessages.Add(messageResponse);
				this.Response.FulfillmentMessages.Add(teleMessageResponse);
				this.Response.FulfillmentMessages.Add(teleMessageQuickReplies);
			}
		}

		private Inventory GetData()
		{
			var telegramId = this.Request.OriginalDetectIntentRequest.Payload.Fields["data"].StructValue.Fields["from"].StructValue.Fields["id"].NumberValue;
			//double telegramId = 9212711;

			Inspector inspector = new Inspector();
			inspector.LoginWithTelegram(telegramId.ToString());

			return inspector.GetFridgeInventory();
		}

	}
}
