using System;
using System.Runtime.CompilerServices;

namespace Vostok.Logging.Tracing
{
    internal static class TracingPrefixFormatter
    {
        private static readonly uint[] Lookup = CreateHexLookup();

        [MethodImpl(MethodImplOptions.NoInlining)]
        public static unsafe string Format(Guid? traceId)
        {
            if (traceId == null)
                return null;

            var chars = stackalloc char[12];
            var traceIdValue = traceId.Value;

            chars[0] = '[';
            chars[1] = 'T';
            chars[2] = '-';
            chars[11] = ']';

            var bytePointer = (byte*) &traceIdValue;

            for (var i = 0; i < 4; i++)
            {
                var hexValue = Lookup[*(bytePointer + 3 - i)];

                chars[3 + 2 * i] = (char) hexValue;
                chars[4 + 2 * i] = (char) (hexValue >> 16);
            }

            return new string(chars, 0, 12);
        }

        private static uint[] CreateHexLookup()
        {
            var result = new uint[256];

            for (var i = 0; i < 256; i++)
            {
                var s = i.ToString("x2");
                result[i] = s[0] + ((uint)s[1] << 16);
            }

            return result;
        }
    }
}