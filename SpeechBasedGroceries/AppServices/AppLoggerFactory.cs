using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SpeechBasedGroceries.AppServices
{
	public class AppLoggerFactory
	{

		private static ILoggerFactory factory = null;

		public static void SetLoggerFactory(ILoggerFactory loggerFactory)
		{
			factory = loggerFactory;
		}

		public static ILogger<T> GetLogger<T>()
		{
			ILogger<T> logger = null;

			if (factory != null)
			{
				logger = factory.CreateLogger<T>();
			}
			else {
				factory = new LoggerFactory();
				logger = factory.CreateLogger<T>();
			}

			return logger;
		}

	}
}
