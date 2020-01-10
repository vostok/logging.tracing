using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Vostok.Tracing.Abstractions;

namespace Vostok.Logging.Tracing
{
    [PublicAPI]
    public static class TracingLogPropertiesFormatter
    {
        [CanBeNull]
        public static unsafe string FormatTraceContext([CanBeNull] TraceContext context)
        {
            if (context == null)
                return null;

            var traceId = context.TraceId;
            var chars = stackalloc char[36];
            var offset = 0;

            chars[offset++] = '[';
            chars[offset++] = 'T';
            chars[offset++] = '-';

            var a = *(int*)&traceId;
            var b = *((short*)&traceId + 2);
            var c = *((short*)&traceId + 3);
            var d = *((byte*)&traceId + 8);
            var e = *((byte*)&traceId + 9);
            var f = *((byte*)&traceId + 10);
            var g = *((byte*)&traceId + 11);
            var h = *((byte*)&traceId + 12);
            var i = *((byte*)&traceId + 13);
            var j = *((byte*)&traceId + 14);
            var k = *((byte*)&traceId + 15);

            offset = HexsToChars(chars, offset, a >> 24, a >> 16);
            offset = HexsToChars(chars, offset, a >> 8, a);
            offset = HexsToChars(chars, offset, b >> 8, b);
            offset = HexsToChars(chars, offset, c >> 8, c);
            offset = HexsToChars(chars, offset, d, e);
            offset = HexsToChars(chars, offset, f, g);
            offset = HexsToChars(chars, offset, h, i);
            offset = HexsToChars(chars, offset, j, k);

            chars[offset] = ']';

            return new string(chars, 0, 36);
        }

        [CanBeNull]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe string FormatSpanIdPrefix([CanBeNull] TraceContext context)
        {
            if (context == null)
                return null;

            var chars = stackalloc char[8];
            var spanId = context.SpanId;

            var a = *(int*)&spanId;

            var offset = 0;

            offset = HexsToChars(chars, offset, a >> 24, a >> 16);
            
            HexsToChars(chars, offset, a >> 8, a);

            return new string(chars, 0, 8);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe int HexsToChars(char* guidChars, int offset, int a, int b)
        {
            guidChars[offset++] = HexToChar(a >> 4);
            guidChars[offset++] = HexToChar(a);
            guidChars[offset++] = HexToChar(b >> 4);
            guidChars[offset++] = HexToChar(b);
            return offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static char HexToChar(int a)
        {
            a = a & 0xf;
            return (char)(a > 9 ? a - 10 + 0x61 : a + 0x30);
        }
    }
}
