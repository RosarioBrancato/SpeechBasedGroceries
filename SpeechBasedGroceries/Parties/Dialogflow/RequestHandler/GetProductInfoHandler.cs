﻿using Google.Cloud.Dialogflow.V2;
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
				this.Response.FulfillmentMessages.Add(this.GetMessage("Sorry, I couldn't check the item's information. Please contact our customer support."));
			}
			else if (products.Count == 0)
			{
				this.Response.FulfillmentMessages.Add(this.GetMessage("The item you are looking for doesn't exist."));
			}
			else if (!products.Any(w => w.NutritionValues.Count > 0))
			{
				this.Response.FulfillmentMessages.Add(this.GetMessage("That item does not have any nutrition values."));
			}
			else
			{
				this.Response.FulfillmentMessages.Add(this.GetMessage("I have found information about the following items:"));

				foreach (var item in products)
				{
					StringBuilder stringBuilder = new StringBuilder();

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

					this.Response.FulfillmentMessages.Add(this.GetMessage(stringBuilder.ToString()));
				}
			}
		}

		private IList<Product> GetProducts()
		{
			IList<Product> products = null;

			var productName = this.Request.QueryResult.Parameters.Fields.GetValueOrDefault("product-name").StringValue;
			if (!string.IsNullOrWhiteSpace(productName))
			{
				Inspector inspector = new Inspector();
				products = inspector.GetItemNutritionalValues(productName);
			}

			return products;
		}
	}
}
