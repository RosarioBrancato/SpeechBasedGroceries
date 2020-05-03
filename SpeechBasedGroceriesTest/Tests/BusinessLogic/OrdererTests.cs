using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceries.Parties.Fridgy;
using SpeechBasedGroceries.Parties.Logistics;
using SpeechBasedGroceriesTest.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeechBasedGroceriesTest.Tests.BusinessLogic
{
    [TestClass()]
    public class OrdererTests : BaseTest
    {
        private Orderer oderer;

        [TestInitialize]
        public void Init()
        {
            this.oderer = new Orderer("356131678");
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.oderer = null;
        }

        [TestMethod()]
        public void PlaceOrderTest()
        {
            string barcode = "7610200428059"; // Bratwurst

            Product product = new FridgyClient().GetProductByBarcode(barcode);
            Delivery delivery = oderer.PlaceOrder(product, 1, "Eine Bestellung");

            Assert.IsNotNull(delivery);
            Assert.IsTrue(delivery.Positions[0].ItemId.Equals(barcode));
        }
    }
}