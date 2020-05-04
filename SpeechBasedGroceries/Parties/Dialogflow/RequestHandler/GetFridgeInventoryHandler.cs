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

		public GetFridgeInventoryHandler(WebhookRequest request, WebhookResponse response) : base(request, response)
		{
		}

		public override void Handle()
		{
			var inventory = this.GetFridgeInventory();

			if (inventory == null)
			{
				this.Response.FulfillmentText = "Sorry, I couldn't check your fridge. Talk to the customer support please.";
			}
			else if (inventory.Items.Count == 0)
			{
				this.Response.FulfillmentText = "Your fridge is empty.";
			}
			else
			{
				//general
				string responseText = "Here’s your current inventory:" + Environment.NewLine + string.Join(Environment.NewLine, inventory.Items.Select(s => string.Concat(s.Quantity, "x ", s.Name)));
				this.Response.FulfillmentText = responseText;

				Intent.Types.Message messageResponse = this.GetMessage(responseText);
				this.Response.FulfillmentMessages.Add(messageResponse);

				//telegram
				Intent.Types.Message teleMessageResponse = this.GetMessage(responseText, Intent.Types.Message.Types.Platform.Telegram);
				this.Response.FulfillmentMessages.Add(teleMessageResponse);

				//Intent.Types.Message teleMessageQuickReplies = this.GetMessageQuickReply("Do you want to order the results?", new[] { "Yes, alphabetically.", "Yes, by due date.", "No." });
				//this.Response.FulfillmentMessages.Add(teleMessageQuickReplies);
			}
		}

		private Inventory GetFridgeInventory()
		{
			//double telegramId = 9212711;
			TelegramUser telegramUser = this.GetTelegramUser();

			Inspector inspector = new Inspector();
			inspector.LoginWithTelegram(telegramUser);

			return inspector.GetFridgeInventory();
		}

	}
}
