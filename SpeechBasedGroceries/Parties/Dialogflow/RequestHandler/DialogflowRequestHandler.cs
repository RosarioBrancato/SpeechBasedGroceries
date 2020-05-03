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
			telegramUser.Id = (int)this.Request.OriginalDetectIntentRequest.Payload.Fields["data"].StructValue.Fields["from"].StructValue.Fields["id"].NumberValue;
			telegramUser.FirstName = this.Request.OriginalDetectIntentRequest.Payload.Fields["data"].StructValue.Fields["from"].StructValue.Fields["first_name"].StringValue;
			telegramUser.LastName = this.Request.OriginalDetectIntentRequest.Payload.Fields["data"].StructValue.Fields["from"].StructValue.Fields["last_name"].StringValue;
			telegramUser.UserName = this.Request.OriginalDetectIntentRequest.Payload.Fields["data"].StructValue.Fields["from"].StructValue.Fields["username"].StringValue;

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
