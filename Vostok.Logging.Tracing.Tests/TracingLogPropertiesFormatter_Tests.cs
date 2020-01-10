using System;
using FluentAssertions;
using NUnit.Framework;
using Vostok.Tracing.Abstractions;

namespace Vostok.Logging.Tracing.Tests
{
    [TestFixture]
    internal class TracingLogPropertiesFormatter_Tests
    {
        [Test]
        public void FormatSpanIdPrefix_should_return_null_for_null_context()
            => TracingLogPropertiesFormatter.FormatSpanIdPrefix(null).Should().BeNull();

        [Test]
        public void FormatSpanIdPrefix_should_produce_same_result_as_a_substring_of_guids_tostring()
        {
            var context = new TraceContext(Guid.NewGuid(), Guid.NewGuid());

            var expectedValue = context.SpanId.ToString().Substring(0, 8);

            TracingLogPropertiesFormatter.FormatSpanIdPrefix(context).Should().Be(expectedValue);
        }

        [Test]
        public void FormatTraceContext_should_return_null_for_null_context()
            => TracingLogPropertiesFormatter.FormatTraceContext(null).Should().BeNull();

        [Test]
        public void FormatTraceContext_should_correctly_format_trace_context_with_full_trace_id()
        {
            var context = new TraceContext(Guid.NewGuid(), Guid.NewGuid());

            var expectedValue = $"[T-{context.TraceId:N}]";

            TracingLogPropertiesFormatter.FormatTraceContext(context).Should().Be(expectedValue);
        }
    }
}