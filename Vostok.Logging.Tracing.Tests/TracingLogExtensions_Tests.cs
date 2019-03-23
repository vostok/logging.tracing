using System;
using FluentAssertions;
using NSubstitute;
using NSubstitute.ReturnsExtensions;
using NUnit.Framework;
using Vostok.Logging.Abstractions;
using Vostok.Tracing.Abstractions;

namespace Vostok.Logging.Tracing.Tests
{
    [TestFixture]
    internal class TracingLogExtensions_Tests
    {
        private ITracer tracer;
        private ILog baseLog;
        private ILog wrappedLog;

        private LogEvent originalEvent;
        private LogEvent observedEvent;

        [SetUp]
        public void TestSetup()
        {
            tracer = Substitute.For<ITracer>();
            tracer.CurrentContext.Returns(new TraceContext(Guid.NewGuid(), Guid.NewGuid()));

            baseLog = Substitute.For<ILog>();
            baseLog
                .When(l => l.Log(Arg.Any<LogEvent>()))
                .Do(info => observedEvent = info.Arg<LogEvent>());

            originalEvent = new LogEvent(LogLevel.Info, DateTimeOffset.Now, null);
            observedEvent = null;

            wrappedLog = baseLog.WithTracingProperties(tracer);
        }

        [Test]
        public void Should_not_modify_incoming_events_when_there_is_no_trace_context()
        {
            tracer.CurrentContext.ReturnsNull();

            wrappedLog.Log(originalEvent);

            observedEvent.Should().BeSameAs(originalEvent);
        }

        [Test]
        public void Should_add_trace_id_property_to_log_events()
        {
            wrappedLog.Log(originalEvent);

            observedEvent.Properties?[TracingLogProperties.TraceId].Should().Be(tracer.CurrentContext?.TraceId);
        }

        [Test]
        public void Should_add_trace_context_property_to_log_events()
        {
            wrappedLog.Log(originalEvent);

            observedEvent.Properties?[WellKnownProperties.TraceContext].Should().Be($"[T-{tracer.CurrentContext?.TraceId.ToString().Substring(0, 8)}]");
        }
    }
}