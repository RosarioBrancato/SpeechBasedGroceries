using Google.Cloud.Dialogflow.V2;
using Microsoft.Extensions.Logging;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
				this.Response.FulfillmentMessages.Add(this.GetMessage("Sorry, I couldn't check your fridge. Please contact our customer support."));
			}
			else if (inventory.Items.Count == 0)
			{
				this.Response.FulfillmentMessages.Add(this.GetMessage("Your fridge is empty."));
			}
			else
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.AppendLine("Here’s your current fridge inventory:");
				foreach (var item in inventory.Items.OrderBy(o => o.QtyType).ThenBy(o => o.Name))
				{
					switch (item.QtyType)
					{
						case Product.QtyTypes.gramm:
							stringBuilder.Append(item.Qty);
							stringBuilder.Append("g     ");
							stringBuilder.Append(item.Name);
							break;

						case Product.QtyTypes.milliliter:
							stringBuilder.Append(item.Qty);
							stringBuilder.Append("ml    ");
							stringBuilder.Append(item.Name);
							break;

						default:
							stringBuilder.Append(item.Quantity);
							stringBuilder.Append("x     ");
							stringBuilder.Append(item.Name);
							break;
					}

					stringBuilder.AppendLine();
				}

				this.Response.FulfillmentMessages.Add(this.GetMessage(stringBuilder.ToString()));
			}
		}

		private Inventory GetFridgeInventory()
		{
			Inventory inventory = null;

			TelegramUser telegramUser = this.GetTelegramUser();
			if (telegramUser.Id > 0)
			{
				Inspector inspector = new Inspector();
				bool success = inspector.LoginWithTelegram(telegramUser);
				if (success)
				{
					inventory = inspector.GetFridgeInventory();
				}
			}

			return inventory;
		}

	}
}
