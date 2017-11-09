using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;

namespace UserManagement.Service.Telemetry
{
    public class Telemetry : Contracts.ITelemetry
    {
		private TelemetryClient _client { get; }

		public Telemetry(TelemetryClient client)
		{
			_client = client;
		}

		public void TrackException(Exception ex)
		{
			_client.TrackException(ex);
		}
		
		public IOperationHolder<RequestTelemetry> StartOperation(string operationName)
		{
			return _client.StartOperation<RequestTelemetry>(operationName);
		}

		public void StopOperation(IOperationHolder<RequestTelemetry> operation)
		{
			_client.StopOperation<RequestTelemetry>(operation);
		}
	}
}
