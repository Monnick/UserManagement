using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;

namespace UserManagement.Service.Telemetry
{
	public class TelemetryLogger : ILogger
	{
		public TelemetryScope CurrentScope { get; set; }

		private TelemetryClient _client { get; }

		public LogLevel ActiveLevel { get; }

		public TelemetryLogger(TelemetryClient client, LogLevel logLevel)
		{
			_client = client;
			ActiveLevel = logLevel;
		}

		public IDisposable BeginScope<TState>(TState state)
		{
			return new TelemetryScope(this, state);
		}

		public bool IsEnabled(LogLevel logLevel)
		{
			return ActiveLevel >= logLevel;
		}

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			switch (logLevel)
			{
				case LogLevel.Trace:
					_client.TrackTrace(formatter(state, exception), SeverityLevel.Verbose);
					break;
				case LogLevel.Debug:
					_client.TrackTrace(formatter(state, exception), SeverityLevel.Verbose);
					break;
				case LogLevel.Information:
					_client.TrackTrace(formatter(state, exception), SeverityLevel.Information);
					break;
				case LogLevel.Warning:
					_client.TrackTrace(formatter(state, exception), SeverityLevel.Warning);
					break;
				case LogLevel.Error:
					_client.TrackTrace(formatter(state, exception), SeverityLevel.Error);
					break;
				case LogLevel.Critical:
					_client.TrackTrace(formatter(state, exception), SeverityLevel.Critical);
					break;
				case LogLevel.None:
					break;
				default:
					break;
			}
		}
	}
}
