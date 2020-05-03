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
		public Delivery PlaceOrder(Product product, int amount, string comment = "")
		{

			Delivery delivery = null;
			if (this.CurrentCustomer != null)
			{

				bool success = TryPayment(this.CurrentCustomer, product, amount);

				if (success)
				{
					delivery = DispatchDelivery(this.CurrentCustomer, product, amount, comment);

				}
				else
				{
					// not enough funds (could technically get here...)
				}

			}
			else
			{
				// customer does not exist
				// should never get here because of constructor
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

			Delivery delivery = new Delivery
			{
				CustomerId = customer.Id,
				Date = DateTime.Now,
				Street = customer.Street,
				Zip = customer.Zip,
				City = customer.City,
				Country = customer.Country,
				Comment = comment
			};

			delivery.Positions = new List<Position>
					{
						new Position
						{
							No = 10,
							ItemId = product.Barcode,
							ItemText = product.Name,
							ItemQty = amount,
							ItemPrice = 0,
							ItemWeight = 0,
							Comment = ""
						}
					};

			return this.LogisticsClient.CreateUpdateDelivery(delivery, true);
		}


	}
}
