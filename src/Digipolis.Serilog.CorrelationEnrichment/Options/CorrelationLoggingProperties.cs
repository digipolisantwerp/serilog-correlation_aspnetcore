using System;

namespace Digipolis.Serilog.CorrelationEnrichment
{
    class CorrelationLoggingProperties
    {
        public const string CorrelationId = "CorrelationId";
        public const string CorrelationSourceId = "CorrelationSourceId";
        public const string CorrelationSourceName = "CorrelationSourceName";
        public const string CorrelationInstanceId = "CorrelationInstanceId";
        public const string CorrelationInstanceName = "CorrelationInstanceName";
        public const string CorrelationHostName = "CorrelationHostName";
        public const string CorrelationUserName = "CorrelationUserName";

        public const string NullValue = "null";
    }
}
