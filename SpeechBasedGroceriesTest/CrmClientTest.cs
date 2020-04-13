using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.Parties.CRM;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceriesTest.Data;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;
using Moq;


namespace SpeechBasedGroceriesTest
{
	[TestClass]
	public class CrmClientTest
	{

		private CrmClient crmClient;


		[TestInitialize]
		public void Init()
		{
			var mock = new Mock<ILogger<CrmClient>>();
			this.crmClient = new CrmClient(mock.Object);
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
			Assert.IsTrue(this.crmClient.GetFridgyTokenOfCustomer("1").Length > 1);
		}


		[TestMethod]
		public void TestCreateUpdateCustomer()
		{
            // DB INSERTS and UPDATES cannot really be tested...
			Customer c1 = new Customer()
			{
				Firstname = "Bob",
                Surname = "Testi",
                Birthdate = DateTime.Parse("1888-12-31"),
                Street = "some",
                Zip = "1234",
                City = "Unitt",
                Country = "Testiopia",
                Email = "yxcv@df.cc",
                TelegramId = 1000001
			};

			var c2 = this.crmClient.CreateUpdateCustomer(c1);
			Assert.IsNotNull(c2);

			c2.Firstname = "Boooob";
			var c3 = this.crmClient.CreateUpdateCustomer(c2);
			Assert.IsTrue(c3.Firstname == c2.Firstname);

		}


	}
}
