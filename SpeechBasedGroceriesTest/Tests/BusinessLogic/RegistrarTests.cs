using Microsoft.Extensions.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SpeechBasedGroceries.BusinessLogic;
using SpeechBasedGroceries.DTOs;
using SpeechBasedGroceriesTest.Tests.Base;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Text;

namespace SpeechBasedGroceriesTest.Tests.BusinessLogic
{
    [TestClass()]
    public class RegistrarTests : BaseTest
    {
        private Registrar reg;

        [TestInitialize]
        public void Init()
        {
            this.reg = new Registrar();
        }

        [TestCleanup]
        public void Cleanup()
        {
            this.reg = null;
        }

        [TestMethod()]
        public void LoginWithTelegramTest()
        {
            Random random = new Random();
            var newTelegram = random.Next(100000000, 999999999).ToString();
            Token token = reg.GetFridgyToken(newTelegram);

            Console.WriteLine(token.Value);
            Assert.IsNotNull(token.Value);

            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token.Value);

            Assert.IsTrue(tokenHandler.CanReadToken(token.Value));
            Assert.AreEqual(default, jwtToken.ValidTo);

        }
    }
}