using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceriesTest.Data;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using System.Linq;
using Moq;
using SpeechBasedGroceries.AppServices;
using SpeechBasedGroceriesTest.Tests.Base;

namespace SpeechBasedGroceriesTest.Tests.Clients
{
	[TestClass]
	public class CrmClientTest : BaseTest
	{

		private CrmClient crmClient;


		[TestInitialize]
		public void Init()
		{
			this.crmClient = new CrmClient();
		}

		[TestCleanup]
		public void Cleanup()
		{
			this.crmClient = null;
		}


		[TestMethod]
		public void TestGetCustomers()
		{
			List<Customer> customers = this.crmClient.GetCustomers();

			Assert.IsTrue(customers.Count > 0);
		}


		[TestMethod]
		public void TestGetCustomerById()
		{
			Assert.IsTrue(this.crmClient.GetCustomerById("1").Id == 1);
			Assert.IsTrue(this.crmClient.GetCustomerById("1a") == null);
		}


		[TestMethod]
		public void TestIsValidId()
		{
			Assert.IsTrue(this.crmClient.IsValidId("1 "));
			Assert.IsTrue(this.crmClient.IsValidId("01"));
			Assert.IsFalse(this.crmClient.IsValidId("1a"));
		}


		[TestMethod]
		public void TestGetFridgyTokenOfCustomer()
		{
			Assert.IsNotNull(this.crmClient.GetCustomerById("2").GetFridigyToken());
		}


		[TestMethod]
		public void TestCreateUpdateDeleteCustomer()
		{
			Customer c1 = UnitTestData.Instance.TestCustomers.ElementAt(0);

			var c2 = this.crmClient.CreateUpdateCustomer(c1);
			Assert.IsNotNull(c2);

			c2.Firstname = "xxxx";
			var c3 = this.crmClient.CreateUpdateCustomer(c2);
			Assert.IsTrue(c3.Firstname == c2.Firstname);

			Assert.IsTrue(crmClient.DeleteCustomer(c3));
			Assert.IsNull(crmClient.GetCustomerById(c3.Id));

		}

		[TestMethod]
		public void TestCreateUpdateDeleteCustomerToken()
		{
			Customer c1 = UnitTestData.Instance.TestCustomers.ElementAt(0);
			c1.Tokens = UnitTestData.Instance.TestTokens;
			c1.Tokens.ForEach(t => t.Creation = DateTime.Now);

			var c2 = this.crmClient.CreateUpdateCustomer(c1, true);
			Assert.IsNotNull(c2);

			Assert.IsTrue(c2.GetFridigyToken().Id > 0);
			Assert.IsTrue(c2.GetFridigyToken().Value == c1.Tokens.ElementAt(0).Value);
			Assert.IsTrue(c2.Tokens.Count == c1.Tokens.Count);

			this.crmClient.DeleteToken(c2.Tokens.ElementAt(1));
			Customer c3 = crmClient.GetCustomerById(c2.Id);
			Assert.IsTrue(c3.Tokens.Count == c2.Tokens.Count - 1);


			Assert.IsTrue(crmClient.DeleteCustomer(c2));
		}

	}
}
