using Microsoft.Extensions.Logging;

namespace CashRegister.Helper
{
	public class ApplicationLogging
	{
		private static ILoggerFactory _Factory = null;

		public static void ConfigureLogger(ILoggerFactory factory)
		{
			factory.AddConsole();
			factory.AddDebug();
			factory.AddFile("Logs/cash-register-{Date}.txt", LogLevel.Error);
		}

		public static ILoggerFactory LoggerFactory
		{
			get
			{
				if (_Factory == null)
				{
					_Factory = new LoggerFactory();
					ConfigureLogger(_Factory);
				}
				return _Factory;
			}
			set { _Factory = value; }
		}
		public static ILogger CreateLogger<T>() => LoggerFactory.CreateLogger<T>();
	}
}
