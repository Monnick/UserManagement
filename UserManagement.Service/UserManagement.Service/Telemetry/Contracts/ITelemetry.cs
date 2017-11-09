using Microsoft.ApplicationInsights.DataContracts;
using Microsoft.ApplicationInsights.Extensibility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Service.Telemetry.Contracts
{
    public interface ITelemetry
    {
		void TrackException(Exception ex);

		IOperationHolder<RequestTelemetry> StartOperation(string operationName);

		void StopOperation(IOperationHolder<RequestTelemetry> operation);
    }
}
