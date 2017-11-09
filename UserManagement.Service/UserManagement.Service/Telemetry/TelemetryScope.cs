using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Telemetry
{
    public class TelemetryScope : IDisposable
    {
		private bool _disposed;

		public TelemetryScope Parent { get; set; }

		public TelemetryLogger Logger { get; set; }

		public object State { get; set; }

		public TelemetryScope(TelemetryLogger logger, object state)
		{
			Logger = logger;
			State = state;
			Parent = logger.CurrentScope;
			logger.CurrentScope = this;
		}

		public void Dispose()
		{
			if (!_disposed)
			{
				_disposed = true;

				if (Parent != null && Logger != null)
					Logger.CurrentScope = Parent;
			}
		}
	}
}
