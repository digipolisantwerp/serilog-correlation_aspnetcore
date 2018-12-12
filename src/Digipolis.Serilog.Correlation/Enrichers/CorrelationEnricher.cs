using System;
using Digipolis.Correlation;
using Digipolis.Serilog.Correlation;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Serilog.Events;

namespace Digipolis.Serilog.Enrichers
{
    public class CorrelationEnricher : ILogEventEnricher
    {
        public CorrelationEnricher(IHttpContextAccessor accessor)
        {
            _accessor = accessor;
        }

        private readonly IHttpContextAccessor _accessor;

        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var httpContext = _accessor.HttpContext;
            var correlationService = httpContext?.RequestServices?.GetService<ICorrelationService>();
            if ( correlationService == null ) return;
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
