using System;
using FluentAssertions;
using NUnit.Framework;

namespace Vostok.Logging.Tracing.Tests
{
    [TestFixture]
    internal class TracingPrefixFormatter_Tests
    {
        [Test]
        public void Should_return_null_for_null_trace_ids()
        {
            TracingPrefixFormatter.Format(null).Should().BeNull();
        }

        [Test]
        public void Should_produce_same_result_as_a_substring_of_guids_tostring()
        {
            var traceId = Guid.NewGuid();

            var expectedValue = $"[T-{traceId.ToString().Substring(0, 8)}]";

            TracingPrefixFormatter.Format(traceId).Should().Be(expectedValue);
        }
    }
}