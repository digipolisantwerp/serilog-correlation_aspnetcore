using System;
using System.Linq;
using Digipolis.Serilog.Enrichers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Core;
using Xunit;

namespace Digipolis.Serilog.Correlation.UnitTests.Enrichers
{
    public class AddCorrelationEnricherExtTests
    {
        [Fact]
        void CorrelationEnricherIsAdded()
        {
            var options = new SerilogExtensionsOptions();
            options.AddCorrelationEnricher();
            Assert.Collection(options.EnricherTypes, item => Assert.Equal(typeof(CorrelationEnricher), item));
        }

        [Fact]
        void CorrelationEnricherIsAddedOnlyOnce()
        {
            var options = new SerilogExtensionsOptions();
            options.AddCorrelationEnricher();
            options.AddCorrelationEnricher();
            Assert.Collection(options.EnricherTypes, item => Assert.Equal(typeof(CorrelationEnricher), item));
        }

        [Fact]
        void CorrelationEnricherIsRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddSerilogExtensions(options => {
                options.MessageVersion = "1";
                options.AddCorrelationEnricher();
            });

            var registrations = services.Where(sd => sd.ServiceType == typeof(ILogEventEnricher) &&
                                                     sd.ImplementationType == typeof(CorrelationEnricher))
                                                     .ToArray();

            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }

        [Fact]
        void IHttpContextAccessorIsRegisteredAsSingleton()
        {
            var services = new ServiceCollection();
            services.AddSerilogExtensions(options => {
                options.MessageVersion = "1";
                options.AddCorrelationEnricher();
            });

            var registrations = services.Where(sd => sd.ServiceType == typeof(IHttpContextAccessor))
                                                     .ToArray();

            Assert.Equal(1, registrations.Count());
            Assert.Equal(ServiceLifetime.Singleton, registrations[0].Lifetime);
        }
    }
}
