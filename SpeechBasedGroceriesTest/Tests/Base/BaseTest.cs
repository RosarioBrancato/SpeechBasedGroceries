using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Debug;
using SpeechBasedGroceries.AppServices;
using System;
using System.Collections.Generic;
using System.Text;

namespace SpeechBasedGroceriesTest.Tests.Base
{
	public abstract class BaseTest
	{

		public BaseTest()
		{
			//settings
			AppSettings.Instance.InitFromJsonFile();

			//logger factory
			LoggerFactory loggerFactory = new LoggerFactory();
			loggerFactory.AddProvider(new DebugLoggerProvider());
			AppLoggerFactory.SetLoggerFactory(loggerFactory);
		}

	}
}
