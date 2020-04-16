using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.Parties.Logistics;
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
	public class LogisticsClientTest
	{

		private LogisticsClient logisticsClient;


		[TestInitialize]
		public void Init()
		{
			var mock = new Mock<ILogger<LogisticsClient>>();
			this.logisticsClient = new LogisticsClient(mock.Object);
		}

		[TestCleanup]
		public void Cleanup()
		{
			this.logisticsClient = null;
		}


		[TestMethod]
		public void TestGetDeliveriesByCustomerId()
		{
			Assert.IsTrue(this.logisticsClient.GetDeliveriesByCustomerId("1").Count > 0);
			Assert.IsTrue(this.logisticsClient.GetDeliveriesByCustomerId("1234567").Count == 0);
		}

		[TestMethod]
		public void TestGetDeliveryById()
		{
			Delivery d1 = this.logisticsClient.GetDeliveryById("100001");

			Assert.IsTrue(d1.Date.Equals(DateTime.Parse("2020-04-09")));
			Assert.IsTrue(d1.Positions.Count > 1);
		}

		[TestMethod]
		public void TestIsValidCustomerNo()
		{
			Assert.IsTrue(this.logisticsClient.IsValidCustomerNo("1 "));
			Assert.IsTrue(this.logisticsClient.IsValidCustomerNo("01"));
			Assert.IsFalse(this.logisticsClient.IsValidCustomerNo("1a"));
		}

		[TestMethod]
		public void TestIsValidDeliveryId()
		{
			Assert.IsTrue(this.logisticsClient.IsValidDeliveryId("100000 "));
			Assert.IsTrue(this.logisticsClient.IsValidDeliveryId("100001"));
			Assert.IsTrue(this.logisticsClient.IsValidDeliveryId("0100000"));
			Assert.IsFalse(this.logisticsClient.IsValidDeliveryId("100000a"));
			Assert.IsFalse(this.logisticsClient.IsValidDeliveryId("99000"));
		}

		[TestMethod]
		public void TestIsValidDate()
		{
			Assert.IsTrue(this.logisticsClient.IsValidDate("2020-04-19"));
			Assert.IsTrue(this.logisticsClient.IsValidDate("2020.04.19"));
			Assert.IsFalse(this.logisticsClient.IsValidDate("29.02.2019"));
			Assert.IsFalse(this.logisticsClient.IsValidDate("33.10.2020"));
		}


		[TestMethod]
		public void TestCreateUpdateDelivery()
		{
			// TODO: outsource test data

			Customer c1 = new CrmClient().GetCustomerById("1");
            Delivery d1 = new Delivery()
			{
				CustomerId = c1.Id,
                Date = DateTime.Today,
                Street = "some",
                Zip = "1234",
                City = "Unitt",
                Country = "Testiopia",
                Comment = "a test delivery...."
			};

            var d2 = this.logisticsClient.CreateUpdateDelivery(d1);
			Assert.IsNotNull(d2);

			d2.Comment = "....delivery test a";
			var d3 = this.logisticsClient.CreateUpdateDelivery(d2);
			Assert.IsTrue(d3.Comment == d2.Comment);
		}


	}
}
