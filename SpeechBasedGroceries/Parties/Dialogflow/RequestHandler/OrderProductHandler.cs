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
			int quantity = (int)this.Request.QueryResult.Parameters.Fields.GetValueOrDefault("quantity")?.NumberValue;

			if (quantity < 1)
			{
				this.Response.FulfillmentMessages.Add(this.GetMessage(quantity + " is not a valid quantity. The minimum quantity is 1."));
			}
			else
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
					string responseText = "Alright, please confirm your order by choosing one of the following option(s).";
					IEnumerable<InlineKeyboardKey> keys = products.Select(s => new InlineKeyboardKey(quantity + "x " + s.Name, "Barcode " + s.Barcode));

					Intent.Types.Message message = this.GetMessageInlineKeyboard(responseText, keys);
					this.Response.FulfillmentMessages.Add(message);
				}
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
