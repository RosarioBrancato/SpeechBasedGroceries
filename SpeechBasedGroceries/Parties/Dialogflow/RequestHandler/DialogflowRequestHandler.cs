using Google.Cloud.Dialogflow.V2;
using SpeechBasedGroceries.DTOs;
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
			if (intentName == DialogflowIntent.GET_FRIDGE_INVENTORY)
			{
				requestHandler = new GetFridgeInventoryHandler(request, response);
			}
			else if (intentName == DialogflowIntent.GET_PRODUCT_INFO)
			{
				requestHandler = new GetProductInfoHandler(request, response);
			}
			else
			{
				requestHandler = new DefaultHandler(request, response);
			}

			return requestHandler;
		}


		public abstract void Handle();


		protected TelegramUser GetTelegramUser()
		{
			TelegramUser telegramUser = new TelegramUser();

			var payload = this.Request.OriginalDetectIntentRequest.Payload;
			if (payload != null && payload.Fields != null && payload.Fields.ContainsKey("data"))
			{
				var data = payload.Fields["data"];
				if (data.StructValue != null && data.StructValue.Fields != null && data.StructValue.Fields.ContainsKey("from"))
				{
					var from = data.StructValue.Fields["from"];
					if (from.StructValue != null && from.StructValue.Fields != null)
					{
						if (from.StructValue.Fields.ContainsKey("id"))
						{
							telegramUser.Id = (int)from.StructValue.Fields["id"].NumberValue;
						}
						if (from.StructValue.Fields.ContainsKey("first_name"))
						{
							telegramUser.FirstName = from.StructValue.Fields["first_name"].StringValue;
						}
						if (from.StructValue.Fields.ContainsKey("last_name"))
						{
							telegramUser.LastName = from.StructValue.Fields["last_name"].StringValue;
						}
						if (from.StructValue.Fields.ContainsKey("username"))
						{
							telegramUser.UserName = from.StructValue.Fields["username"].StringValue;
						}
					}
				}
			}

			return telegramUser;
		}

		protected Intent.Types.Message GetMessage(string text, Intent.Types.Message.Types.Platform platform = Intent.Types.Message.Types.Platform.Unspecified)
		{
			Intent.Types.Message messageResponse = new Intent.Types.Message();
			messageResponse.Text = new Intent.Types.Message.Types.Text();
			messageResponse.Text.Text_.Add(text);

			if (platform != Intent.Types.Message.Types.Platform.Unspecified)
			{
				messageResponse.Platform = platform;
			}

			return messageResponse;
		}

		protected Intent.Types.Message GetMessageQuickReply(string title, string[] quickReplies)
		{
			Intent.Types.Message teleMessageQuickReplies = new Intent.Types.Message();
			teleMessageQuickReplies.Platform = Intent.Types.Message.Types.Platform.Telegram;
			teleMessageQuickReplies.QuickReplies = new Intent.Types.Message.Types.QuickReplies();
			teleMessageQuickReplies.QuickReplies.Title = title;

			foreach (string quickReply in quickReplies)
			{
				teleMessageQuickReplies.QuickReplies.QuickReplies_.Add(quickReply);
			}

			return teleMessageQuickReplies;
		}

	}
}
