using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceriesTest.Tests.Base;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeechBasedGroceriesTest.Tests.BusinessLogic
{
    [TestClass()]
    public class InspectorTests : BaseTest
    {

        private Inspector inspector;

        [TestInitialize]
        public void Init()
        {
            this.inspector = new Inspector();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.inspector = null;
        }

        [TestMethod()]
        public void GetFridgeInventoryTest()
        {
            Random random = new Random();

            TelegramUser telegramUser = new TelegramUser();
            telegramUser.Id = random.Next(100000000, 999999999);

            inspector.LoginWithTelegram(telegramUser);
            Inventory inv = inspector.GetFridgeInventory();
            Assert.IsNotNull(inv);
        }
    }
}