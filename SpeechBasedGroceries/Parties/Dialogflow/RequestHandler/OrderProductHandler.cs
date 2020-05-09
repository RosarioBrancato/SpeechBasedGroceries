using Google.Cloud.Dialogflow.V2;
using Newtonsoft.Json.Linq;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.Dialogflow.RequestHandler
{
	public class OrderProductHandler : DialogflowRequestHandler
	{
		public OrderProductHandler(WebhookRequest request, WebhookResponse response) : base(request, response)
		{
		}

		public override void Handle()
		{
			IList<Product> products = this.GetProducts();

			if (products == null)
			{
				this.Response.FulfillmentMessages.Add(this.GetMessage("Sorry, I cannot process your order. Please contact our customer support."));
			}
			else if (products.Count == 0)
			{
				this.Response.FulfillmentMessages.Add(this.GetMessage("The item you are looking for doesn't exist."));
			}
			else
			{
				string responseText = "Which of these products would you like to order?";
				IEnumerable<InlineKeyboardKey> keys = products.Select(s => new InlineKeyboardKey(s.Name, "Barcode " + s.Barcode));

				Intent.Types.Message message = this.GetMessageInlineKeyboard(responseText, keys);
				this.Response.FulfillmentMessages.Add(message);
			}
		}

		private IList<Product> GetProducts()
		{
			IList<Product> products = null;

			var productName = this.Request.QueryResult.Parameters.Fields.GetValueOrDefault("product-name")?.StringValue;
			if (!string.IsNullOrWhiteSpace(productName))
			{
				Inspector inspector = new Inspector();
				products = inspector.GetItemNutritionalValues(productName);
			}

			return products;
		}
	}
}
