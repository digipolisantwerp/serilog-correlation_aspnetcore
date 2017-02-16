using System;
using System.Collections.Generic;
using Digipolis.Correlation;
using Digipolis.Serilog.Enrichers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Serilog.Parsing;
using Xunit;

namespace Digipolis.Serilog.Correlation.UnitTests.Enrichers
{
    public class CorrelationEnricherEnrichTests
    {
        [Fact]
        void CorrelationIdIsAdded()
        {
            var accessor = CreateHttpContextAccessor();
            var enricher = new CorrelationEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, null);

            Assert.Contains(CorrelationLoggingProperties.CorrelationId, logEvent.Properties.Keys);
        }

        [Fact]
        void CorrelationSourceIdIsAdded()
        {
            var accessor = CreateHttpContextAccessor();
            var enricher = new CorrelationEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, null);

            Assert.Contains(CorrelationLoggingProperties.CorrelationSourceId, logEvent.Properties.Keys);
        }

        [Fact]
        void CorrelationSourceNameIsAdded()
        {
            var accessor = CreateHttpContextAccessor();
            var enricher = new CorrelationEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, null);

            Assert.Contains(CorrelationLoggingProperties.CorrelationSourceName, logEvent.Properties.Keys);
        }

        [Fact]
        void CorrelationInstanceIdIsAdded()
        {
            var accessor = CreateHttpContextAccessor();
            var enricher = new CorrelationEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, null);

            Assert.Contains(CorrelationLoggingProperties.CorrelationInstanceId, logEvent.Properties.Keys);
        }

        [Fact]
        void CorrelationInstanceNameIsAdded()
        {
            var accessor = CreateHttpContextAccessor();
            var enricher = new CorrelationEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, null);

            Assert.Contains(CorrelationLoggingProperties.CorrelationInstanceName, logEvent.Properties.Keys);
        }

        [Fact]
        void CorrelationHostNameIsAdded()
        {
            var accessor = CreateHttpContextAccessor();
            var enricher = new CorrelationEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, null);

            Assert.Contains(CorrelationLoggingProperties.CorrelationHostName, logEvent.Properties.Keys);
        }

        [Fact]
        void CorrelationUserNameIsAdded()
        {
            var accessor = CreateHttpContextAccessor();
            var enricher = new CorrelationEnricher(accessor);
            var logEvent = CreateLogEvent();

            enricher.Enrich(logEvent, null);

            Assert.Contains(CorrelationLoggingProperties.CorrelationUserName, logEvent.Properties.Keys);
        }

        private IHttpContextAccessor CreateHttpContextAccessor()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.AddScoped<ICorrelationContext, CorrelationContext>();
            services.Configure<CorrelationOptions>(opt => opt.SourceHeaderKey = "123");

            var accessor = new HttpContextAccessor();
            accessor.HttpContext = new DefaultHttpContext();
            accessor.HttpContext.RequestServices = services.BuildServiceProvider();

            return accessor;
        }

        private LogEvent CreateLogEvent()
        {
            var tokens = new List<MessageTemplateToken>();
            var properties = new List<LogEventProperty>();
            var logEvent = new LogEvent(DateTime.Now, LogEventLevel.Information, null, new MessageTemplate(tokens), properties);
            return logEvent;
        }
    }
}
