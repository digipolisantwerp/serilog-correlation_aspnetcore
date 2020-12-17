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
            if (correlationService == null) return;

            // Do not enrich events originating from Digipolis.Correlation toolbox to avoid recursive enrichment causing a StackOverflowException
            LogEventPropertyValue sourceContext;
            var isLogFromCorrelationToolbox = logEvent.Properties.TryGetValue("SourceContext", out sourceContext) && sourceContext.ToString().StartsWith("\"Digipolis.Correlation.");
            if (isLogFromCorrelationToolbox) return;

            var ctx = correlationService.GetContext();

            var correlationIdProp = new LogEventProperty(CorrelationLoggingProperties.CorrelationId, new ScalarValue(ctx.Id ?? CorrelationLoggingProperties.NullValue));

            logEvent.AddOrUpdateProperty(correlationIdProp);
            
        }
    }
}
