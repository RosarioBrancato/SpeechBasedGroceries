using Google.Cloud.Dialogflow.V2;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.Parties.Dialogflow.RequestHandler
{
	public class GetProductInfoHandler : DialogflowRequestHandler
	{

		public GetProductInfoHandler(WebhookRequest request, WebhookResponse response) : base(request, response)
		{
		}

		public override void Handle()
		{
			IList<Product> products = this.GetProducts();

			if (products == null)
			{
				this.Response.FulfillmentText = "Sorry, I couldn't check the item's information.";
			}
			else if (products.Count == 0)
			{
				this.Response.FulfillmentText = "The item you are looking for doesn't exist.";
			}
			else if (!products.Any(w => w.NutritionValues.Count > 0))
			{
				this.Response.FulfillmentText = "That item does not have any nutrition values.";
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("I have found information about the following items: ");

				foreach (var item in products)
				{
					stringBuilder.AppendLine();
					stringBuilder.Append(item.Name);
					stringBuilder.Append(" has: ");

					bool first = true;
					foreach (var value in item.NutritionValues)
					{
						if (first)
						{
							first = false;
						}
						else
						{
							stringBuilder.Append(", ");
						}

						stringBuilder.Append(value.Value);
						stringBuilder.Append(" ");
						stringBuilder.Append(value.Name);
					}
					stringBuilder.AppendLine();
				}

				//general
				string responseText = stringBuilder.ToString();
				this.Response.FulfillmentText = responseText;

				Intent.Types.Message messageResponse = this.GetMessage(responseText);
				this.Response.FulfillmentMessages.Add(messageResponse);

				//telegram
				Intent.Types.Message teleMessageResponse = this.GetMessage(responseText, Intent.Types.Message.Types.Platform.Telegram);
				this.Response.FulfillmentMessages.Add(teleMessageResponse);
			}
		}

		private IList<Product> GetProducts()
		{
			Inspector inspector = new Inspector();

			var productName = this.Request.QueryResult.Parameters.Fields.GetValueOrDefault("product-name").StringValue;

			return inspector.GetItemNutritionalValues(productName);
		}
	}
}
