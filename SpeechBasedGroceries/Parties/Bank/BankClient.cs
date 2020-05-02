using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.AppServices;


namespace SpeechBasedGroceries.Parties.Bank
{
	public class BankClient
	{

		private BankDAO bankDao = new BankDAO();
		private readonly ILogger<BankClient> _logger;


		public BankClient()
		{
			_logger = AppLoggerFactory.GetLogger<BankClient>();
		}


		// only used for unit tests
		public BankClient(ILogger<BankClient> logger)
		{
			_logger = logger;
		}


		
		public List<Transaction> GetTransactionsByCustomer(int customerId)
		{
			return bankDao.GetTransactionsOfCustomer(customerId);
		}


        public bool IssuePayment(int customerId, Transaction t)
        {
			// returns true if sufficent credit available
			List<Transaction> transaction = GetTransactionsByCustomer(customerId);
			transaction.Add(t);
			return transaction.Sum(t => t.Amount) >= 0;
		}


		public Transaction GetRandomTransaction(bool positive = false, int amount = 0)
		{
			return bankDao.GetRandomTransaction(positive, amount);
		}

	}
}
