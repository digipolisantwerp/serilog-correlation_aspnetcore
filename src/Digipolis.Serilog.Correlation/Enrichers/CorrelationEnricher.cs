using Digipolis.Correlation;
using Digipolis.Serilog.Correlation;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;
using System;

namespace Digipolis.Serilog.Enrichers
{
    public class CorrelationEnricher : ILogEventEnricher
    {
        public CorrelationEnricher(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        private readonly IServiceProvider _serviceProvider;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var correlationService = _serviceProvider?.GetService<ICorrelationService>();
            if (correlationService == null) return;

            // Do not enrich events originating from Digipolis.Correlation toolbox to avoid recursive enrichment causing a StackOverflowException
            LogEventPropertyValue sourceContext;
            var isLogFromCorrelationToolbox = logEvent.Properties.TryGetValue("SourceContext", out sourceContext) && sourceContext.ToString().StartsWith("\"Digipolis.Correlation.");
            if (isLogFromCorrelationToolbox) return;

            // get correlation context from HTTPContext or create a new one for current application (ex. for logging within background processes)
            var ctx = correlationService.GetContext();

            var correlationIdProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationId, new ScalarValue(ctx.Id ?? CorrelationLoggingProperties.NullValue));
            var sourceIdProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationSourceId, new ScalarValue(ctx.SourceId ?? CorrelationLoggingProperties.NullValue));
            var sourceNameProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationSourceName, new ScalarValue(ctx.SourceName ?? CorrelationLoggingProperties.NullValue));
            var instanceIdProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationInstanceId, new ScalarValue(ctx.InstanceId ?? CorrelationLoggingProperties.NullValue));
            var instanceNameProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationInstanceName, new ScalarValue(ctx.InstanceName ?? CorrelationLoggingProperties.NullValue));
            var hostNameProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationHostName, new ScalarValue(ctx.IpAddress ?? CorrelationLoggingProperties.NullValue));
            var userNameProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationUserName, new ScalarValue(ctx.UserId ?? CorrelationLoggingProperties.NullValue));

            logEvent.AddOrUpdateProperty(correlationIdProp);
            logEvent.AddOrUpdateProperty(sourceIdProp);
            logEvent.AddOrUpdateProperty(sourceNameProp);
            logEvent.AddOrUpdateProperty(instanceIdProp);
            logEvent.AddOrUpdateProperty(instanceNameProp);
            logEvent.AddOrUpdateProperty(hostNameProp);
            logEvent.AddOrUpdateProperty(userNameProp);
        }
    }
}
