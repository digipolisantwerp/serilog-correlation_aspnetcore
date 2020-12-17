using Digipolis.Correlation;
using Digipolis.Serilog.Enrichers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Events;
using Serilog.Parsing;
using System;
using System.Collections.Generic;
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

        private IHttpContextAccessor CreateHttpContextAccessor()
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<CorrelationOptions>(opt => opt.CorrelationHeaderRequired = true);
            
            var correlationService = new Moq.Mock<ICorrelationService>();
            correlationService.Setup(x => x.GetContext()).Returns(new CorrelationContext
            {
                DgpHeader = "eyJpZCI6ImEsInNvdXJjZUlkIjpiLCJzb3VyY2VOYW1lIjpjLCJpbnN0YW5jZUlkIjpkLCJpbnN0YW5jZU5hbWUiOmUsInVzZXJJZCI6ZiwiaXBBZGRyZXNzIjoiZyJ9",
                Id = "a",
                SourceId = "b",
                SourceName = "c",
                InstanceId = "d",
                InstanceName = "e",
                UserId = "f",
                IpAddress = "g"
            });

            services.AddSingleton(correlationService.Object);

            var accessor = new HttpContextAccessor
            {
                HttpContext = new DefaultHttpContext()
            };
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
