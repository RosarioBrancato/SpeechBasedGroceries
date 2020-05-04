using Google.Cloud.Dialogflow.V2;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json.Linq;
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
			else if (intentName == DialogflowIntent.ORDER_PRODUCT)
			{
				requestHandler = new OrderProductHandler(request, response);
			}
			else if (intentName == DialogflowIntent.EXECUTE_PRODUCT_ORDER)
			{
				requestHandler = new ExecuteProductOrderHandler(request, response);
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

			Struct payload = this.Request.OriginalDetectIntentRequest.Payload;
			if (payload != null && payload.Fields != null && payload.Fields.ContainsKey("data"))
			{
				var data = payload.Fields["data"];

				if (data.StructValue != null && data.StructValue.Fields != null)
				{
					Value from = null;
					if (data.StructValue.Fields.ContainsKey("callback_query"))
					{
						var callBackQuery = data.StructValue.Fields["callback_query"];

						if (callBackQuery != null && callBackQuery.StructValue.Fields != null)
						{
							from = callBackQuery.StructValue.Fields["from"];
						}
					}
					else if (data.StructValue.Fields.ContainsKey("from"))
					{
						from = data.StructValue.Fields["from"];
					}

					if (from != null && from.StructValue != null && from.StructValue.Fields != null)
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
			Intent.Types.Message messageQuickReply = new Intent.Types.Message();
			messageQuickReply.Platform = Intent.Types.Message.Types.Platform.Telegram;
			messageQuickReply.QuickReplies = new Intent.Types.Message.Types.QuickReplies();
			messageQuickReply.QuickReplies.Title = title;

			foreach (string quickReply in quickReplies)
			{
				messageQuickReply.QuickReplies.QuickReplies_.Add(quickReply);
			}

			return messageQuickReply;
		}

		protected Intent.Types.Message GetMessageCards(string title, string[] cards)
		{
			Intent.Types.Message messageCard = new Intent.Types.Message();
			messageCard.Platform = Intent.Types.Message.Types.Platform.Telegram;
			messageCard.Card = new Intent.Types.Message.Types.Card();
			messageCard.Card.Title = title;

			foreach (string card in cards)
			{
				Intent.Types.Message.Types.Card.Types.Button button = new Intent.Types.Message.Types.Card.Types.Button();
				button.Text = card;
				button.Postback = "Card click: " + card;
				messageCard.Card.Buttons.Add(button);
			}

			return messageCard;
		}

		protected Intent.Types.Message GetMessageInlineKeyboard(string title, IEnumerable<InlineKeyboardKey> keys)
		{
			Intent.Types.Message messageCard = new Intent.Types.Message();
			messageCard.Platform = Intent.Types.Message.Types.Platform.Telegram;

			JArray inlineKeyBoard = new JArray();
			foreach (var key in keys)
			{
				JObject entryObject = new JObject();
				entryObject.Add("text", key.Text);
				entryObject.Add("callback_data", key.CallbackData);

				JArray entryArr = new JArray();
				entryArr.Add(entryObject);

				inlineKeyBoard.Add(entryArr);
			}

			JObject replyMarkup = new JObject();
			replyMarkup.Add("inline_keyboard", inlineKeyBoard);

			JObject telegram = new JObject();
			telegram.Add("text", title);
			telegram.Add("reply_markup", replyMarkup);

			JObject payload = new JObject();
			payload.Add("telegram", telegram);

			messageCard.Payload = Struct.Parser.ParseJson(payload.ToString());

			return messageCard;
		}

	}
}
