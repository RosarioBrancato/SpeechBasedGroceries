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
	public class RegistrarTest : BaseTest
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
		public void TestLoginWithTelegram()
		{
			Random random = new Random();
			string uuid = Guid.NewGuid().ToString().Substring(0, 4);

			TelegramUser telegramUser = new TelegramUser();
			telegramUser.Id = random.Next(100000000, 999999999);
			telegramUser.FirstName = "Max-" + uuid;
			telegramUser.LastName = "Muster-" + uuid;

			Customer customer = this.reg.RegisterTelegramUser(telegramUser);
			Token token = customer.GetFridigyToken();

			Assert.IsNotNull(token.Value);

			JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
			JwtSecurityToken jwtToken = tokenHandler.ReadJwtToken(token.Value);

			Assert.IsTrue(tokenHandler.CanReadToken(token.Value));
			//db does not save the time
			Assert.AreEqual(token.Expiration.Value.Date, jwtToken.ValidTo.Date);
		}
	}
}