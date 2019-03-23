using JetBrains.Annotations;

namespace Vostok.Logging.Tracing
{
    /// <summary>
    /// Contains names of well-known tracing log properties.
    /// </summary>
    [PublicAPI]
    public static class TracingLogProperties
    {
        /// <summary>
        /// Current trace identifier.
        /// </summary>
        public const string TraceId = "TraceId";
    }
}