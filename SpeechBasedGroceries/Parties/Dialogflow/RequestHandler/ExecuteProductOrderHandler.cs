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
	public class ExecuteProductOrderHandler : DialogflowRequestHandler
	{

		public ExecuteProductOrderHandler(WebhookRequest request, WebhookResponse response) : base(request, response)
		{
		}

		public override void Handle()
		{
			Delivery delivery = this.PlaceOrder();

			if (delivery != null && delivery.Positions.Count > 0)
			{
				Position position = delivery.Positions.FirstOrDefault();
				this.Response.FulfillmentText = "Thank you for your order. Your " + position.ItemQty + " " + position.ItemText + " can be now seen in your fridge.";
			}
			else
			{
				this.Response.FulfillmentText = "Sorry, I cannot process your order.  Please contact our customer support.";
			}
		}

		private Delivery PlaceOrder()
		{
			Delivery delivery = null;
			string barcode = string.Empty;
			int quantity = -1;

			Google.Protobuf.WellKnownTypes.Value barcodeValue;
			bool success = this.Request.QueryResult.Parameters.Fields.TryGetValue("barcode", out barcodeValue);
			if (success)
			{
				barcode = barcodeValue.NumberValue.ToString();
			}

			Google.Protobuf.WellKnownTypes.Value quantityValue;
			var context = this.Request.QueryResult.OutputContexts.Where(w => w.Name.Contains("orderproduct-followup")).FirstOrDefault();
			success = context.Parameters.Fields.TryGetValue("quantity", out quantityValue);
			if (success)
			{
				quantity = (int)quantityValue.NumberValue;
			}

			if (!string.IsNullOrWhiteSpace(barcode) && quantity > 0)
			{
				TelegramUser telegramUser = this.GetTelegramUser();

				Orderer orderer = new Orderer();
				success = orderer.LoginWithTelegram(telegramUser);
				if (success)
				{
					delivery = orderer.PlaceOrder(barcode, quantity, "Telegram Order");
				}
			}

			return delivery;
		}
	}
}
