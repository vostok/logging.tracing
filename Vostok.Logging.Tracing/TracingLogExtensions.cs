using System;
using JetBrains.Annotations;
using Vostok.Logging.Abstractions;
using Vostok.Tracing.Abstractions;

namespace Vostok.Logging.Tracing
{
    [PublicAPI]
    public static class TracingLogExtensions
    {
        /// <summary>
        /// <para>Returns a log that enriches all incoming <see cref="LogEvent"/>s with following properties:</para>
        /// <list type="bullet">
        ///     <item><description><see cref="TracingLogProperties.TraceId"/> with the value of current trace identifier (<see cref="Guid"/>).</description></item>
        ///     <item><description><see cref="WellKnownProperties.TraceContext"/></description></item>
        /// </list>
        /// </summary>
        [NotNull]
        public static ILog WithTracingProperties([NotNull] this ILog log, [NotNull] ITracer tracer)
        {
            log = log.WithProperty(TracingLogProperties.TraceId, () => tracer.CurrentContext?.TraceId);
            log = log.WithProperty(WellKnownProperties.TraceContext, () => TracingLogPropertiesFormatter.FormatTraceContext(tracer.CurrentContext));

            return log;
        }
    }
}