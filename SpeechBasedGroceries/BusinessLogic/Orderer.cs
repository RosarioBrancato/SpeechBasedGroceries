using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.Parties.Bank;
using SpeechBasedGroceries.Parties.Logistics;
using SpeechBasedGroceries.Parties.Fridgy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeechBasedGroceries.BusinessLogic.Base;

namespace SpeechBasedGroceries.BusinessLogic
{
	public class Orderer : CiuBase
	{

		/// <summary>
		/// Returns a Delivery if successful and Null if unsuccessful
		/// </summary>
		/// <param name="product"></param>
		/// <param name="amount"></param>
		/// <param name="comment"></param>
		/// <returns></returns>
		public Delivery PlaceOrder(string productBarcode, int amount, string comment = "")
		{
			Delivery delivery = null;

			if (this.CurrentCustomer != null)
			{
				this.FridgyClient.SetToken(this.CurrentCustomer.GetFridigyToken().Value);
				Product product = this.FridgyClient.GetProductByBarcode(productBarcode);

				if (product != null)
				{
					bool success = this.TryPayment(this.CurrentCustomer, product, amount);
					if (success)
					{
						delivery = this.DispatchDelivery(this.CurrentCustomer, product, amount, comment);
						// add product to the user's fridge
						this.UpdateFridge(product, amount);
					}
				}
			}

			return delivery;
		}


		private bool TryPayment(Customer customer, Product product, int amount)
		{
			// issue fictional transaction
			Transaction transaction = GetTransaction(product, amount);

			// try if transaction can be placed
			return this.BankClient.IssuePayment(customer.Id, transaction);
		}


		private Transaction GetTransaction(Product product, int amount)
		{
			// TODO: how to get the real pricing?
			return this.BankClient.GetRandomTransaction();
		}


		private Delivery DispatchDelivery(Customer customer, Product product, int amount, string comment)
		{
			// currently every delivery has only one position

			Delivery delivery = new Delivery();
			delivery.CustomerId = customer.Id;
			delivery.Date = DateTime.Now;
			delivery.Street = customer.Street;
			delivery.Zip = customer.Zip;
			delivery.City = customer.City;
			delivery.Country = customer.Country;
			delivery.Comment = comment;

			Position position = new Position();
			position.No = 10;
			position.ItemId = product.Barcode;
			position.ItemText = product.Name;
			position.ItemQty = amount;
			position.ItemPrice = 0;
			position.ItemWeight = 0;
			position.Comment = "";

			delivery.Positions.Add(position);

			return this.LogisticsClient.CreateUpdateDelivery(delivery, true);
		}

		private void UpdateFridge(Product product, int amount)
		{
			Token token = this.CurrentCustomer.GetFridigyToken();
			if (token != null && !string.IsNullOrWhiteSpace(token.Value))
			{
				this.FridgyClient.SetToken(token.Value);
				for (int i = 0; i < amount; i++)
				{
					string id = this.FridgyClient.PutItemInFridge(product);
				}
			}
		}


	}
}
