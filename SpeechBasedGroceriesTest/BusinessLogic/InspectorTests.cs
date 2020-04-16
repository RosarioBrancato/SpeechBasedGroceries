﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeechBasedGroceries.BusinessLogic.Tests
{
    [TestClass()]
    public class InspectorTests
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
            var newTelegram = random.Next(100000000, 999999999).ToString();
            inspector.LoginWithTelegram(newTelegram);
            Inventory inv = inspector.GetFridgeInventory();
            Assert.IsNotNull(inv);
        }
    }
}