using System;
using Digipolis.Serilog.Enrichers;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Serilog.Core;

namespace Digipolis.Serilog
{
    public static class AddCorrelationEnricherExt
    {
        public static SerilogExtensionsOptions AddCorrelationEnricher(this SerilogExtensionsOptions options)
        {
            options.ApplicationServices.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            options.ApplicationServices.AddSingleton<ILogEventEnricher, CorrelationEnricher>();
            return options;
        }
    }
}
