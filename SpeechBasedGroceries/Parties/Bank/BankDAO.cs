using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Threading.Tasks;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceries.DTOs;
using Microsoft.Extensions.Logging;
using Microsoft.CodeAnalysis.FlowAnalysis;
using Microsoft.VisualBasic.CompilerServices;

namespace SpeechBasedGroceries.Parties.Bank
{
    public class BankDAO
    {

		// private readonly ILogger<CrmDAO> _logger;


		public BankDAO()
		{
		}

        #region transaction queries

        public List<Transaction> GetTransactionsOfCustomer(int customerId)
		{
			Random rand = new Random();

			List<Transaction> transactions = new List<Transaction>();

			// add fictional credit
			transactions.Add(GetRandomTransaction(true, 250));


            // create fictional purchases
			int j = rand.Next(0, 8);
			for (int i = 0 ; i < j ; i++)
			{
				transactions.Add(GetRandomTransaction());
			}

			return transactions;
		}



		public Transaction GetRandomTransaction(bool positive = false, int amount = 0)
		{
			Random rand = new Random();
			Transaction t = new Transaction
			{
				Id = rand.Next(),
				Date = DateTime.Now,
				Amount = (double)(rand.Next(100, 2000) / 100)
			};

			t.Amount = amount > 0 ? amount : t.Amount;
			t.Amount = positive ? t.Amount : t.Amount * -1;

			return t;
		}

		#endregion


	}
}
